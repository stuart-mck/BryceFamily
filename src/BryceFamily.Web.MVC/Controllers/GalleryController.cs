using BryceFamily.Repo.Core.Files;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Read.FamilyEvents;
using BryceFamily.Repo.Core.Read.Gallery;
using BryceFamily.Repo.Core.Read.ImageReference;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Web.MVC.Infrastructure.Alerts;
using BryceFamily.Web.MVC.Infrastructure.Authentication;
using BryceFamily.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Controllers
{

    public class GalleryController : BaseController
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly IGalleryReadRepository _readModel;
        private readonly IFamilyEventReadRepository _familyEventReadRepository;
        private readonly IWriteRepository<Repo.Core.Model.Gallery, Guid> _writeModel;
        private readonly IWriteRepository<ImageReference, Guid> _imageReferenceWriteModel;
        private readonly IImageReferenceReadRepository _imageReferenceReadRepository;
        private readonly IFileService _fileService;
        private readonly ClanAndPeopleService _clanAndPeopleService;
        private readonly ContextService _contextService;

        public GalleryController(ILogger<GalleryController> logger, IGalleryReadRepository readModel, IFamilyEventReadRepository familyEventReadRepository, IWriteRepository<Repo.Core.Model.Gallery, Guid> galleryWriteModel, IWriteRepository<Repo.Core.Model.ImageReference, Guid> imageReferenceWriteModel, IImageReferenceReadRepository imageReferenceReadRepository, IFileService fileService, ClanAndPeopleService clanAndPeopleService, ContextService contextService):base("Galleries", "gallery")
        {
            _logger = logger;
            _readModel = readModel;
            _familyEventReadRepository = familyEventReadRepository;
            _writeModel = galleryWriteModel;
            _imageReferenceWriteModel = imageReferenceWriteModel;
            _imageReferenceReadRepository = imageReferenceReadRepository;
            _fileService = fileService;
            _clanAndPeopleService = clanAndPeopleService;
            _contextService = contextService;
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            var gallery = await _readModel.Load(id, CancellationToken.None);

            return View(await Models.Gallery.Map(gallery, _clanAndPeopleService, _familyEventReadRepository, _imageReferenceReadRepository, CancellationToken.None));
        }

        public async Task<IActionResult> Index()
        {
            var galleries = await _readModel.LoadAll(CancellationToken.None);

            var readModel = galleries.Select(
                g => Models.Gallery.Map(g, _clanAndPeopleService,_familyEventReadRepository, _imageReferenceReadRepository, CancellationToken.None).Result);

            return View(readModel);
            
        }

        [HttpGet, Route("Gallery/ViewImage/{galleryId}/{imageId}")]
        public async Task<IActionResult> ViewImage(Guid galleryId, Guid imageId)
        {
            try
            {
                var cancellationToken = CancellationToken.None;
                var galleryImages = (await _imageReferenceReadRepository.LoadByGallery(galleryId, cancellationToken)).ToList();

                var img = await _imageReferenceReadRepository.Load(galleryId, imageId, cancellationToken);
                var index = galleryImages.IndexOf(galleryImages.FirstOrDefault(t => t.ImageID == imageId));

                return View(new ImageViewModel()
                {
                    Description = img.Description,
                    GalleryId = galleryId,
                    ImageId = imageId,
                    Title = img.Title,
                    PreviousLink = IndexIsNotFirst(index) ? galleryImages[index - 1].ImageID : Guid.Empty,
                    NextLink =  IndexIsNotLast(index, galleryImages.Count())  ? galleryImages[index + 1].ImageID : Guid.Empty
                });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private bool IndexIsNotLast(int index, int gallerySize)
        {
            var length = gallerySize;
            return index < length - 1;
        }

        private bool IndexIsNotFirst(int index)
        {
            return index != 0;
        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult EditGallery()
        {
            return View(new Models.Gallery()
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now
            });
        }

     

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult FamilyGallery()
        {
            return View(new FamilyGalleryCreateModel(_clanAndPeopleService.Clans.ToList()));
        }


        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> FamilyGallery(FamilyGalleryCreateModel newGallery)
        {
            var gallery = new Repo.Core.Model.Gallery()
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Name = newGallery.Name,
                Summary = newGallery.Description,
                FamilyId = newGallery.FamilyId,
                Owner = _contextService.LoggedInPerson.Id
            };

            await _writeModel.Save(gallery, new CancellationToken());
            return View(new FamilyGalleryCreateModel(_clanAndPeopleService.Clans.ToList())).WithSuccess("Gallery saved");
        }


        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult NewGallery()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> EventGallery()
        {
            var events = (await _familyEventReadRepository.GetAllEvents(CancellationToken.None)).Select(Models.FamilyEvent.Map);
            return View(new EventGalleryCreateModel(events));
        }


        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> EventGallery(EventGalleryCreateModel newGallery)
        {
            var gallery = new Repo.Core.Model.Gallery()
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Name = newGallery.Name,
                Summary = newGallery.Description,
                FamilyEvent = newGallery.FamilyEventId,
                Owner = _contextService.LoggedInPerson.Id 
            };
            await _writeModel.Save(gallery, new CancellationToken());
            var events = (await _familyEventReadRepository.GetAllEvents(CancellationToken.None)).Select(Models.FamilyEvent.Map);
            return View(new EventGalleryCreateModel(events)).WithSuccess("Gallery saved"); 
        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> EditGalleryImages(Guid id)
        {

            var cancellationToken = new CancellationToken();

            var gallery =await  Models.Gallery.Map(await _readModel.Load(id, cancellationToken), _clanAndPeopleService, _familyEventReadRepository, _imageReferenceReadRepository,  cancellationToken);

            return View(new FileUploadModel()
            {
                GalleryId = gallery.Id,
                GalleryName = gallery.Title
            });
        }

        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult EditGallery(Models.Gallery gallery)
        {
            if (!ModelState.IsValid) return BadRequest();

            _writeModel.Save(gallery.MapToEntity(), new CancellationToken());
            return  RedirectToAction("Index").WithSuccess("Gallery saved"); ;
        }

        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> UploadFiles(Guid galleryId, List<IFormFile> files)
        {
            try
            {
                var allowedExtensions = new[] { ".png", ".gif", ".jpg" };
                

                var cancellationToken = new CancellationToken();

                var gallery = await  _readModel.Load(galleryId, cancellationToken);
                if (gallery == null)
                    return BadRequest("Ïnvalid Gallery Id");

                foreach(var formFile in files)
                {
                    var checkextension = Path.GetExtension(formFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(checkextension))
                        return BadRequest($"Invalid file format {checkextension} on file {formFile.FileName}");


                    var img = new ImageReferenceModel()
                    {
                        MimeType = formFile.ContentType,
                        Title = formFile.FileName,
                        Id = Guid.NewGuid(),
                        GalleryReference = galleryId
                    };

                    if (formFile.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(stream);
                            img.Reference = await _fileService.SaveFile(img.Id, galleryId.ToString(), stream, formFile.FileName, formFile.ContentType, cancellationToken);

                            stream.Position = 0;
                            var resized = FileResizer.GetFileResized(ReadFully(stream), 150);
                            await _fileService.SaveFile(img.Id, $"{galleryId}/thumbnail", resized, formFile.FileName, formFile.ContentType, cancellationToken);

                            await _imageReferenceWriteModel.Save(img.MapToEntity(), cancellationToken);
                        }
                    }
                }
                return await Task.FromResult(RedirectToAction("EditGalleryImages", new { id = gallery.ID}).WithSuccess("Image/s saved")); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload the image/s");
                return RedirectToAction("EditGalleryImages", new { id = galleryId }).WithError("Sorry - there has been an error saving the files. The error has been logged");
            }
            
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}