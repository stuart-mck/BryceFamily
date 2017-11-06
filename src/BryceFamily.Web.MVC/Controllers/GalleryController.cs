using BryceFamily.Repo.Core.Files;
using BryceFamily.Web.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Repo.Core.Read.Gallery;
using BryceFamily.Repo.Core.Read.FamilyEvents;
using BryceFamily.Repo.Core.Read.ImageReference;
using System.IO;

namespace BryceFamily.Web.MVC.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryReadRepository _readModel;
        private readonly IFamilyEventReadRepository _familyEventReadRepository;
        private readonly IWriteRepository<Repo.Core.Model.Gallery, Guid> _writeModel;
        private readonly IWriteRepository<ImageReference, Guid> _imageReferenceWriteModel;
        private readonly IImageReferenceReadRepository _imageReferenceReadRepository;
        private readonly IFileService _fileService;

        public GalleryController(IGalleryReadRepository readModel, IFamilyEventReadRepository familyEventReadRepository, IWriteRepository<Repo.Core.Model.Gallery, Guid> galleryWriteModel, IWriteRepository<Repo.Core.Model.ImageReference, Guid> imageReferenceWriteModel, IImageReferenceReadRepository imageReferenceReadRepository, IFileService fileService)
        {
            _readModel = readModel;
            _familyEventReadRepository = familyEventReadRepository;
            _writeModel = galleryWriteModel;
            _imageReferenceWriteModel = imageReferenceWriteModel;
            this._imageReferenceReadRepository = imageReferenceReadRepository;
            _fileService = fileService;
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            var gallery = await _readModel.Load(id, CancellationToken.None);

            return View(Models.Gallery.Map(gallery,_familyEventReadRepository, CancellationToken.None));
        }

        public async Task<IActionResult> Index()
        {
            var galleries = await _readModel.LoadAll(CancellationToken.None);

            var readModel = galleries.Select(
                g => Models.Gallery.Map(g, _familyEventReadRepository, CancellationToken.None).Result);

            return View(readModel);
            
        }

        [HttpGet]
        public async Task<IActionResult> ViewImage(Guid galleryId, Guid imageId)
        {
            try
            {
                var cancellationToken = CancellationToken.None;
                var galleryImages = (await _imageReferenceReadRepository.LoadByGallery(galleryId, cancellationToken)).ToList();

                var img = await _imageReferenceReadRepository.Load(galleryId, imageId, cancellationToken);
                var index = galleryImages.IndexOf(img);

                return View(new ImageViewModel()
                {
                    Description = img.Description,
                    GalleryId = galleryId,
                    ImageId = imageId,
                    Title = img.Title,
                    PreviousLink = IndexIsNotFirst(index) ? galleryImages[index - 1].ID : Guid.Empty,
                    NextLink =  IndexIsNotLast(index, galleryImages.Count())  ? galleryImages[index + 1].ID : Guid.Empty
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
        public IActionResult EditGallery()
        {
            return View(new Models.Gallery()
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now
            });
        }

        [HttpGet]
        public IActionResult NewGallery()
        {
            return View(new GalleryCreateModel());
        }

        [HttpPost]
        public IActionResult NewGallery(GalleryCreateModel newGallery)
        {
            var gallery = new Repo.Core.Model.Gallery()
            {
                ID = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Name = newGallery.Name,
                Summary = newGallery.Description
            };
            _writeModel.Save(gallery, new CancellationToken());
            return View(new GalleryCreateModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditGalleryImages(Guid id)
        {

            var cancellationToken = new CancellationToken();

            var gallery =await  Models.Gallery.Map(await _readModel.Load(id, cancellationToken), _familyEventReadRepository, cancellationToken);

            return View(new FileUploadModel()
            {
                GalleryId = gallery.Id,
                GalleryName = gallery.Title
            });
        }

        [HttpPost]
        public IActionResult EditGallery(Models.Gallery gallery)
        {
            if (!ModelState.IsValid) return BadRequest();

            _writeModel.Save(gallery.MapToEntity(), new CancellationToken());
            return  RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(Guid galleryId, List<IFormFile> files)
        {
            try
            {
                var cancellationToken = new CancellationToken();

                var gallery = await  _readModel.Load(galleryId, cancellationToken);
                if (gallery == null)
                    return BadRequest("Ïnvalid Gallery Id");

                files.ForEach(async formFile =>
                {
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
                            img.Reference = await _fileService.SaveFile(img.Id, galleryId, stream, formFile.FileName, formFile.ContentType, cancellationToken);
                            await _imageReferenceWriteModel.Save(img.MapToEntity(), cancellationToken);
                        }   
                    }
                });

                
                return await Task.FromResult(RedirectToAction("EditGalleryImages", new { id = gallery.ID}));
            }
            catch (Exception)
            {
                return BadRequest("Gallery does not exist");
            }
            
        }
    }
}