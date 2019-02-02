using BryceFamily.Repo.Core.Emails;
using BryceFamily.Repo.Core.Files;
using BryceFamily.Repo.Core.Read.FamilyEvents;
using BryceFamily.Repo.Core.Read.Gallery;
using BryceFamily.Repo.Core.Read.ImageReference;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Web.MVC.Infrastructure.Authentication;
using BryceFamily.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using BryceFamily.Web.MVC.Models.Image;

namespace BryceFamily.Web.MVC.Controllers
{
    public class EventController : BaseController
    {
        private readonly IFamilyEventReadRepository _readmodel;
        private readonly IWriteRepository<Repo.Core.Model.FamilyEvent, Guid> _writeModel;
        private readonly IImageReferenceReadRepository _imageReferenceReadRepository;
        private readonly IGalleryReadRepository _galleryReadRepository;
        private readonly ContextService _contextService;
        private readonly IWriteRepository<Repo.Core.Model.Gallery, Guid> _galleryWriteRepository;
        private readonly IFileService _fileService;
        private readonly IWriteRepository<Repo.Core.Model.ImageReference, Guid> _imageReferenceWriteModel;
        private readonly ISesService _sesServive;
        private readonly ClanAndPeopleService _clanAndPeopleService;

        public EventController(IFamilyEventReadRepository readmodel,
                               IWriteRepository<Repo.Core.Model.FamilyEvent, Guid> writeModel,
                               IImageReferenceReadRepository imageReferenceReadRepository,
                               IGalleryReadRepository galleryReadRepository,
                               ContextService contextService,
                               IWriteRepository<Repo.Core.Model.Gallery, Guid> galleryWriteRepository,
                               IFileService fileService,
                               IWriteRepository<Repo.Core.Model.ImageReference, Guid> imageReferenceWriteModel,
                               ISesService sesServive,
                               ClanAndPeopleService clanAndPeopleService)
            : base("Significant Events", "events")
        {
            _readmodel = readmodel;
            _writeModel = writeModel;
            _imageReferenceReadRepository = imageReferenceReadRepository;
            _galleryReadRepository = galleryReadRepository;
            _contextService = contextService;
            _galleryWriteRepository = galleryWriteRepository;
            _fileService = fileService;
            _imageReferenceWriteModel = imageReferenceWriteModel;
            _sesServive = sesServive;
            _clanAndPeopleService = clanAndPeopleService;
        }

        [AllowAnonymous]
        [Route("Event")]
        [Route("Event/Index")]
        // GET: /<controller>/
        public IActionResult Index()
        {
            //var events = (await _readmodel.GetAllEvents(new CancellationToken())).ToList();
            //return View(events.Select(e => Models.FamilyEvent.Map(e)));
            return View();
        }

        [AllowAnonymous]
        [Route("Event/Reunions")]
        // GET: /<controller>/
        public async Task<IActionResult> Reunions()
        {
            var events = (await _readmodel.GetAllEvents(new CancellationToken())).Where(t => t.EventType == Repo.Core.Model.EventType.Reunion).ToList();
            return View(events.Select(e => Models.FamilyEvent.Map(e)));
        }

        [AllowAnonymous]
        [Route("Event/Events")]
        // GET: /<controller>/
        public async Task<IActionResult> Events()
        {
            var events = (await _readmodel.GetAllEvents(new CancellationToken())).Where(t => t.EventType != Repo.Core.Model.EventType.Reunion).ToList();
            return View(events.Select(e => Models.FamilyEvent.Map(e)));
        }

        [HttpGet("Event/{eventId}")]
        public async Task<IActionResult> Detail(Guid eventId)
        {
            var cancellationToken = GetCancellationToken();
            var familyEvent = await _readmodel.Load(eventId, cancellationToken);
            var gallery = await _galleryReadRepository.FindAllByFamilyEvent(familyEvent.ID, cancellationToken);
            var imageReference = (await _imageReferenceReadRepository.LoadByGallery(gallery.FirstOrDefault(g => g.DefaultFamilyEventGallery).ID, cancellationToken)).FirstOrDefault(ir => ir.DefaultGalleryImage);
            return View(Models.FamilyEvent.MapWithImageReference(familyEvent, imageReference.ID, gallery.FirstOrDefault(g => g.DefaultFamilyEventGallery).ID,  imageReference.Title));

        }

        [HttpGet("Event/Edit/{eventId}")]
        public async Task<IActionResult> Edit(Guid eventId)
        {
            var cancellationToken = GetCancellationToken();
            var familyEvent = await _readmodel.Load(eventId, cancellationToken);
            var gallery = await _galleryReadRepository.FindAllByFamilyEvent(familyEvent.ID, cancellationToken);
            var imageReference = (await _imageReferenceReadRepository.LoadByGallery(gallery.FirstOrDefault(g => g.DefaultFamilyEventGallery).ID, cancellationToken)).FirstOrDefault(ir => ir.DefaultGalleryImage);
            var fe = imageReference == null ? FamilyEvent.Map(familyEvent) : FamilyEvent.MapWithImageReference(familyEvent, imageReference.ID, gallery.FirstOrDefault(g => g.DefaultFamilyEventGallery).ID, imageReference.Title);
            return View("EditEvent", fe);
        }

        [HttpGet("Event/NewEvent")]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult NewEvent()
        {
            return View("EditEvent", new Models.FamilyEvent()
            {
                EntityId = Guid.NewGuid(),
                EventStatus = eventStatus.Pending,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                GalleryId = Guid.NewGuid()
            });
        }

        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> NewEvent(Models.FamilyEvent familyEventPost)
        {
            if (ModelState.IsValid)
            {
                var cancellationToken = GetCancellationToken();

                var galleries = await _galleryReadRepository.FindAllByFamilyEvent(familyEventPost.EntityId, cancellationToken);

                if (galleries == null || !galleries.Where(g => g.DefaultFamilyEventGallery == true).Any())
                {
                    await _galleryWriteRepository.Save(new Repo.Core.Model.Gallery
                    {
                        ID = familyEventPost.GalleryId,
                        DateCreated = DateTime.Now,
                        DefaultFamilyEventGallery = true,
                        FamilyEvent = familyEventPost.EntityId,
                        Owner = _contextService.LoggedInPerson.Id,
                        Name = familyEventPost.Title
                    }, cancellationToken);
                }

                await _writeModel.Save(familyEventPost.MapToEntity(), cancellationToken);
                return RedirectToAction("SendEmail", new { id = familyEventPost.EntityId});
            }
            return BadRequest();
            
        }

        [HttpGet]
        public async Task<IActionResult> SendEmail(Guid id)
        {
            var @event = await _readmodel.Load(id, GetCancellationToken());
            return View(new SendEventEmailModel { EventId = @event.ID, EventTitle = @event.Title });
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEventEmailModel sendEventEmailModel)
        {
            var cancellationToken = GetCancellationToken();
            var @event = await _readmodel.Load(sendEventEmailModel.EventId, cancellationToken);

            foreach(var subscriber in _clanAndPeopleService.People.Where(s => s.SubscribeToEmail))
            {
                await _sesServive.SendEmail(subscriber.EmailAddress, BuildEmailContent(@event), sendEventEmailModel.Subject, cancellationToken);
            }

            return View(new SendEventEmailModel { EventId = @event.ID, EventTitle = @event.Title });
        }

        private string BuildEmailContent(Repo.Core.Model.FamilyEvent @event)
        {
            var sb = new StringBuilder();

            sb.Append("<table border=\"0\" valign=\"top\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><thead><tr><th>A new event has been added to the Bryce Family Web Site</th></tr></thead>");
            sb.Append($"<tr><tbody><td colspan=\"2\"><h2>{@event.Title}</h2></td></tr>");
            sb.Append($"<tr><td colspan=\"2\">{@event.Details}</td></tr>");
            sb.Append($"<tr><td colspan=\"2\"><table border=\"0\" valign=\"top\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">");
            sb.Append($"<tr><td>Location:</td><td></td>{@event.Location?.Address1}</tr>");
            if (!string.IsNullOrEmpty(@event?.Location?.Address2))
            {
                sb.Append($"<tr><td>&nbsp;</td><td></td>{@event.Location?.Address2}</tr>");
            }
            sb.Append($"<tr><td>&nbsp;</td><td></td>{@event.Location?.City}</tr>");
            sb.Append($"<tr><td>&nbsp;</td><td></td>{@event.Location?.State} {@event.Location?.PostCode}</tr>");
            sb.Append($"</table></td></tr>");
            sb.Append($"<tr><td>Start Date:{@event.StartDate:dd-MMM-yyyy}</td><td>End Date: {@event.EndDate:dd-MMM-yyyy}</td></tr>");
            sb.Append($"<tr><td>Organiser:{@event.OrganiserName}</td><td>Email: {@event.OrganiserEmail}</td></tr>");
            sb.Append($"<tr><td>For more details head to <a href=\"http://www.brycefamily.net/event/{@event.ID}\">here<a></td></tr>");
            sb.Append($"</table>");
            return sb.ToString();
        }

        //[HttpGet]
        //public async Task<IActionResult> EventImage(Guid eventId)
        //{
        //    var @event = await _readmodel.Load(eventId, GetCancellationToken());
        //    return View("EditorTemplates/EventImage", new EventImage(@event.ID));
        //}

        [HttpPost("Event/EventImage")]
        public async Task<IActionResult> EventImage(FamilyEventImage imageWriteModel)
        {
            var cancellationToken = GetCancellationToken();
            var gallery = await _galleryReadRepository.FindAllByFamilyEvent(imageWriteModel.FamilyEventId, GetCancellationToken());

            if (!(gallery.Any() || gallery.Any(t => t.DefaultFamilyEventGallery)))
            {
                // is there an imagegallery for this event?
                var eventGallery = new Repo.Core.Model.Gallery()
                    {
                        ID = imageWriteModel.FamilyEventGalleryId,
                        DateCreated = DateTime.Now,
                        FamilyEvent = imageWriteModel.FamilyEventId,
                        DefaultFamilyEventGallery = true,
                        Name  = string.Empty,
                        Owner = _contextService.LoggedInPerson.Id
                    };
                await _galleryWriteRepository.Save(eventGallery, cancellationToken);
                imageWriteModel.FamilyEventGalleryId = eventGallery.ID;
            }
            else  
            {
                imageWriteModel.FamilyEventGalleryId = gallery.First(g => g.DefaultFamilyEventGallery).ID;
                var imageReferences = await _imageReferenceReadRepository.LoadByGallery(imageWriteModel.FamilyEventGalleryId, cancellationToken);
                var currentDefaultImage = imageReferences.FirstOrDefault(t => t.DefaultGalleryImage);
                if (currentDefaultImage != null)
                {
                    currentDefaultImage.DefaultGalleryImage = false;
                    await _imageReferenceWriteModel.Save(currentDefaultImage, cancellationToken);
                }
            }

            var allowedExtensions = new[] { ".png", ".gif", ".jpg" };

            var checkextension = Path.GetExtension(imageWriteModel.DefaultImage.FileName).ToLower();

            if (!allowedExtensions.Contains(checkextension))
                return BadRequest($"Invalid file format {checkextension} on file {imageWriteModel.DefaultImage.FileName}");

            var img = new ImageReferenceModel()
            {
                MimeType = imageWriteModel.DefaultImage.ContentType,
                Title = imageWriteModel.DefaultImage.FileName ,
                Id = Guid.NewGuid(),
                GalleryReference = imageWriteModel.FamilyEventGalleryId,
                DefaultGalleryImage = true
            };

            if (imageWriteModel.DefaultImage.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await imageWriteModel.DefaultImage.CopyToAsync(stream);
                    stream.Position = 0;
                    var resized = FileResizer.GetFileResized(ReadFully(stream), 150);
                    await _fileService.SaveFile(img.Id, $"{imageWriteModel.FamilyEventGalleryId}/thumbnail", resized, imageWriteModel.DefaultImage.FileName, imageWriteModel.DefaultImage.ContentType, cancellationToken);

                    await _imageReferenceWriteModel.Save(img.MapToEntity(), cancellationToken);
                }
            }
            
            return Json(new { path = img.Id + "/thumbnail/" + img.Title, id = img.Id });
        }
    }
}
        