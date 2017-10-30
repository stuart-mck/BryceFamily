using BryceFamily.Repo.Core.Files;
using BryceFamily.Repo.Core.Repository;
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

namespace BryceFamily.Web.MVC.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryReadRepository _readModel;
        private readonly IFamilyEventReadRepository _familyEventReadRepository;
        private readonly IWriteRepository<Repo.Core.Model.Gallery, Guid> _writeModel;
        private readonly IFileService _fileService;

        public GalleryController(IGalleryReadRepository readModel, IFamilyEventReadRepository familyEventReadRepository, IWriteRepository<Repo.Core.Model.Gallery, Guid> writeModel, IFileService fileService)
        {
            _readModel = readModel;
            _familyEventReadRepository = familyEventReadRepository;
            _writeModel = writeModel;
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
                var gallery = await _readModel.Load(galleryId, CancellationToken.None);

                var img = gallery.ImageReferences.First(ir => ir.ID == imageId);
                var index = gallery.ImageReferences.IndexOf(img);

                return View(new ImageViewModel()
                {
                    Description = img.Description,
                    GalleryId = galleryId,
                    ImageId = imageId,
                    Title = img.Title,
                    PreviousLink = IndexIsNotFirst(index) ? gallery.ImageReferences[index - 1].ID : Guid.Empty,
                    NextLink =  IndexIsNotLast(index, gallery )  ? gallery.ImageReferences[index + 1].ID : Guid.Empty
                });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private bool IndexIsNotLast(int index, Repo.Core.Model.Gallery gallery)
        {
            var length = gallery.ImageReferences.Count;
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
                ImageReferences = new List<ImageReference>(),
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

                var gallery = await Models.Gallery.Map(await _readModel.Load(galleryId, cancellationToken), _familyEventReadRepository, cancellationToken);

                files.ForEach(async file =>
                {
                    var img = new ImageReferenceModel()
                    {
                        MimeType = file.ContentType,
                        Title = file.Name,
                        Id = Guid.NewGuid()
                    };
                    gallery.ImageReferences.Add(img);

                    if (file.Length > 0)
                    {
                        img.Reference = await _fileService.SaveFile(img.Id, galleryId, file, file.Name, cancellationToken);
                    }
                });

                await _writeModel.Save(gallery.MapToEntity(), cancellationToken);

                return await Task.FromResult(RedirectToAction("EditGalleryImages", new { id = gallery.Id}));
            }
            catch (Exception)
            {
                return BadRequest("Gallery does not exist");
            }
            
        }
    }
}