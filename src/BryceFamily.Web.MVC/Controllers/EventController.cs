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

        public EventController(IFamilyEventReadRepository readmodel, IWriteRepository<Repo.Core.Model.FamilyEvent, Guid> writeModel, IImageReferenceReadRepository imageReferenceReadRepository, IGalleryReadRepository galleryReadRepository, ContextService contextService, IWriteRepository<Repo.Core.Model.Gallery, Guid> galleryWriteRepository, IFileService fileService, IWriteRepository<Repo.Core.Model.ImageReference, Guid> imageReferenceWriteModel) :base("Family Events", "events")
        {
            _readmodel = readmodel;
            _writeModel = writeModel;
            _imageReferenceReadRepository = imageReferenceReadRepository;
            _galleryReadRepository = galleryReadRepository;
            _contextService = contextService;
            _galleryWriteRepository = galleryWriteRepository;
            _fileService = fileService;
            _imageReferenceWriteModel = imageReferenceWriteModel;
        }

        [AllowAnonymous]
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var events = (await _readmodel.GetAllEvents( new CancellationToken())).ToList();
            
            return View(events.Select(FamilyEvent.Map));
        }

        public async Task<IActionResult> Detail(Guid eventId)
        {
            var familyEvent = await _readmodel.Load(eventId, new CancellationToken());
            return View(FamilyEvent.Map(familyEvent));

        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult NewEvent()
        {
            return View("EditEvent", new FamilyEvent()
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
        public async Task<IActionResult> NewEvent(FamilyEvent familyEventPost)
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
            return View(FamilyEvent.Map(@event));
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(Guid eventId, string emailSubject, string from)
        {
            
            return View(FamilyEvent.Map(@event));
        }



        [HttpGet]
        public async Task<IActionResult> EventImage(Guid eventId)
        {
            var @event = await _readmodel.Load(eventId, GetCancellationToken());
            return View("EditorTemplates/EventImage", new EventImage(@event.ID));
        }

        [HttpPost]
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
            }
            else  
            {
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
                    img.Reference = await _fileService.SaveFile(img.Id, imageWriteModel.FamilyEventGalleryId.ToString(), stream, imageWriteModel.DefaultImage.FileName, imageWriteModel.DefaultImage.ContentType, cancellationToken);
                    stream.Position = 0;
                    var resized = FileResizer.GetFileResized(ReadFully(stream), 150);
                    await _fileService.SaveFile(img.Id, $"{imageWriteModel.FamilyEventGalleryId}/thumbnail", resized, imageWriteModel.DefaultImage.FileName, imageWriteModel.DefaultImage.ContentType, cancellationToken);

                    await _imageReferenceWriteModel.Save(img.MapToEntity(), cancellationToken);
                }
            }
            
            return Json(new { path = img.Reference + "/thumbnail/" + img.Title, id = img.Id });
        }
    }
}
        