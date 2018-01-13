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

        public EventController(IFamilyEventReadRepository readmodel, IWriteRepository<Repo.Core.Model.FamilyEvent, Guid> writeModel, IImageReferenceReadRepository imageReferenceReadRepository, IGalleryReadRepository galleryReadRepository, ContextService contextService, IWriteRepository<Repo.Core.Model.Gallery, Guid> galleryWriteRepository) :base("Family Events", "events")
        {
            _readmodel = readmodel;
            _writeModel = writeModel;
            _imageReferenceReadRepository = imageReferenceReadRepository;
            _galleryReadRepository = galleryReadRepository;
            _contextService = contextService;
            _galleryWriteRepository = galleryWriteRepository;
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
                return RedirectToAction("Index");
            }
            return BadRequest();
            
        }

    }
}
        