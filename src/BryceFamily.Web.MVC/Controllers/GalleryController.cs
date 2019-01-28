using BryceFamily.Repo.Core.Files;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Read.FamilyEvents;
using BryceFamily.Repo.Core.Read.Gallery;
using BryceFamily.Repo.Core.Read.ImageReference;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Web.MVC.Models.Image;
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
using System.Drawing;
using System.Drawing.Imaging;

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

        public GalleryController(ILogger<GalleryController> logger, 
                                 IGalleryReadRepository readModel, 
                                 IFamilyEventReadRepository familyEventReadRepository, 
                                 IWriteRepository<Repo.Core.Model.Gallery, Guid> galleryWriteModel, 
                                 IWriteRepository<Repo.Core.Model.ImageReference, Guid> imageReferenceWriteModel, 
                                 IImageReferenceReadRepository imageReferenceReadRepository, 
                                 IFileService fileService, 
                                 ClanAndPeopleService clanAndPeopleService,
                                 ContextService contextService)
            :base("Galleries", "gallery")
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
            var cancellationToken = GetCancellationToken();

            var gallery = await _readModel.Load(id, cancellationToken);

            return View(await Models.Gallery.Map(gallery, _clanAndPeopleService, _familyEventReadRepository, _imageReferenceReadRepository, cancellationToken));
        }

        public async Task<IActionResult> Index()
        {
            var cancellationToken = GetCancellationToken();
            var galleries = (await _readModel.LoadAll(cancellationToken));

            var readModel = galleries.Select(
                g => Models.GalleryIndex.Map(g, _clanAndPeopleService, _familyEventReadRepository, cancellationToken).Result);

            return View(readModel.ToList());
            
        }

        [HttpGet, Route("Gallery/ViewImage/{galleryId}/{imageId}")]
        public async Task<IActionResult> ViewImage(Guid galleryId, Guid imageId)
        {
            try
            {
                var cancellationToken = CancellationToken.None;
                var galleryImages = (await _imageReferenceReadRepository.LoadByGallery(galleryId, cancellationToken)).OrderBy(t => t.LastUpdated).ToList();

                var img = await _imageReferenceReadRepository.Load(galleryId, imageId, cancellationToken);
                var index = galleryImages.IndexOf(galleryImages.FirstOrDefault(t => t.ImageID == imageId));

                return View(new ImageViewModel()
                {
                    Description = img.Description,
                    GalleryId = galleryId,
                    ImageId = imageId,
                    Title = img.Title,
                    FileName = img.FileName,
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
        public async Task<IActionResult> FamilyGallery()
        {
            var cancellationToken = GetCancellationToken();
            var events = await _familyEventReadRepository.GetAllEvents(cancellationToken);

            return View(new FamilyGalleryCreateModel(_clanAndPeopleService.Clans.ToList(), 
                                                    events.Select(Models.FamilyEvent.Map).ToList()));
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
                FamilyEvent = newGallery.EventId,
                Owner = _contextService.LoggedInPerson.Id,
                GalleryDate = newGallery.GalleryDate
            };

            await _writeModel.Save(gallery, new CancellationToken());
            return RedirectToAction("Index").WithSuccess("Gallery saved");
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

            var cancellationToken = CancellationToken.None;

            await _writeModel.Save(gallery, cancellationToken);
            var events = (await _familyEventReadRepository.GetAllEvents(cancellationToken)).Select(Models.FamilyEvent.Map);
            return View(new EventGalleryCreateModel(events)).WithSuccess("Gallery saved"); 
        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> EditGalleryImages(Guid id)
        {

            var cancellationToken = GetCancellationToken();

            var gallery =await  Models.Gallery.Map(await _readModel.Load(id, cancellationToken), _clanAndPeopleService, _familyEventReadRepository, _imageReferenceReadRepository,  cancellationToken);
            

            return View(new FileUploadModel()
            {
                GalleryId = gallery.Id,
                GalleryName = gallery.Title,
                ClanMembers = _clanAndPeopleService.People.Where(t => t.ClanId == gallery.Clan?.Id && !t.IsSpouse)
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

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        [Route("Gallery/EditImage/{galleryId}/{imageId}")]
        public async Task<IActionResult> EditImage(Guid galleryId, Guid imageId)
        {
            var cancellationToken = GetCancellationToken();
            var imgReference = await _imageReferenceReadRepository.Load(galleryId, imageId, cancellationToken);

            return View(ImageReferenceModel.Map(imgReference));
        }


        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        [Route("Gallery/EditImage/{galleryId}/{imageId}")]
        public async Task<IActionResult> EditImage(ImageReferenceModel imageReferenceModel)
        {
            var cancellationToken = GetCancellationToken();
            var imgReference = await _imageReferenceReadRepository.Load(imageReferenceModel.GalleryReference, imageReferenceModel.Id, cancellationToken);


            var imgRefToSave = new ImageReference
            {
                DefaultGalleryImage = imageReferenceModel.DefaultGalleryImage,
                Description = imageReferenceModel.Description,
                FileName = string.IsNullOrEmpty(imgReference.FileName) ? imgReference.Title : imgReference.FileName,
                //GalleryId = imageReferenceModel.GalleryReference,
                ID = imgReference.ID,
                ImageID = imgReference.ImageID,
                ImageLocation = imgReference.ImageLocation,
                MimeType = imgReference.MimeType,
                PersonId = imageReferenceModel.PersonId,
                Title = imageReferenceModel.Title
            };


            await _imageReferenceWriteModel.Save(imgRefToSave, cancellationToken);

            return RedirectToAction("Detail", new { id = imgRefToSave.ID });
        }

        private static string[] _AllowedExtensions = new[] { ".png", ".gif", ".jpg" };

        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IActionResult> UploadFiles(Guid galleryId, int FamilyImageId, List<IFormFile> files)
        {
            try
            {
                var cancellationToken = GetCancellationToken();

                var gallery = await  _readModel.Load(galleryId, cancellationToken);
                if (gallery == null)
                    return BadRequest("Ïnvalid Gallery Id");

                foreach(var formFile in files)
                {
                    var checkextension = Path.GetExtension(formFile.FileName).ToLower();

                    if (!_AllowedExtensions.Contains(checkextension))
                        return BadRequest($"Invalid file format {checkextension} on file {formFile.FileName}");

                    var img = new ImageReferenceModel()
                    {
                        MimeType = formFile.ContentType,
                        FileName = formFile.FileName,
                        Id = Guid.NewGuid(),
                        GalleryReference = galleryId,
                        PersonId = FamilyImageId
                    };

                    if (formFile.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(stream);
                            var rotatedImage = RotateImage(stream);
                            using (var rotatedStream = new MemoryStream())
                            {
                                rotatedImage.Save(rotatedStream, GetRawFormat(formFile.FileName));
                                img.Reference = await _fileService.SaveFile(img.Id, galleryId.ToString(), rotatedStream, formFile.FileName, formFile.ContentType, cancellationToken);

                                rotatedStream.Position = 0;
                                var resized = FileResizer.GetFileResized(ReadFully(rotatedStream), 150);
                                await _fileService.SaveFile(img.Id, $"{galleryId}/thumbnail", resized, formFile.FileName, formFile.ContentType, cancellationToken);

                                await _imageReferenceWriteModel.Save(img.MapToEntity(), cancellationToken);
                            }
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

        private ImageFormat GetRawFormat(string fileName)
        {
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".gif":
                    return ImageFormat.Gif;
                case ".png":
                    return ImageFormat.Png;
                default:
                    throw new Exception("Invalid file type");
                    
            }
        }

        private Image RotateImage(Stream imageStream)
        {
            Image originalImage = Image.FromStream(imageStream);

            if (originalImage.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = originalImage.GetPropertyItem(0x0112).Value[0];
                switch (rotationValue)
                {
                    case 1: // landscape, do nothing
                        break;

                    case 8: // rotated 90 right
                            // de-rotate:
                        originalImage.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipNone);
                        break;

                    case 3: // bottoms up
                        originalImage.RotateFlip(rotateFlipType: RotateFlipType.Rotate180FlipNone);
                        break;

                    case 6: // rotated 90 left
                        originalImage.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }
            return originalImage;
        }
    }
}