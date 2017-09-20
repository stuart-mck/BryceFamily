using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Repo.Core.Repository;
using BryceFamily.Web.MVC.Models;
using Microsoft.AspNetCore.Http;
using BryceFamily.Repo.Core.Files;

namespace BryceFamily.Web.MVC.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IReadModel<Repo.Core.Model.Gallery, Guid> _readModel;
        private readonly IWriteModel<Repo.Core.Model.Gallery, Guid> _writeModel;
        private readonly IFileService _fileService;

        public GalleryController(IReadModel<Repo.Core.Model.Gallery, Guid> readModel, IWriteModel<Repo.Core.Model.Gallery, Guid> writeModel, IFileService fileService)
        {
            _readModel = readModel;
            _writeModel = writeModel;
            _fileService = fileService;
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            var gallery = await _readModel.Load(id, CancellationToken.None);

            return View(Gallery.Map(gallery));
        }

        public async Task<IActionResult> Index()
        {
            var galleries = (await _readModel.AsQueryable()).ToList();

            return View(galleries.Select(Gallery.Map));
            
        }

        [HttpGet]
        public IActionResult EditGallery()
        {
            return View(new Gallery()
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditGalleryImages(Guid id)
        {
            var gallery = Gallery.Map(await _readModel.Load(id, CancellationToken.None));

            return View(new FileUploadModel()
            {
                GalleryId = gallery.Id,
                GalleryName = gallery.Title
            });
        }

        [HttpPost]
        public IActionResult EditGallery(Gallery gallery)
        {
            if (!ModelState.IsValid) return BadRequest();

            _writeModel.Save(gallery.MapToEntity());
            return  RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(Guid galleryId, List<IFormFile> files)
        {
            try
            {
                var gallery = Gallery.Map(await _readModel.Load(galleryId, CancellationToken.None));

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
                        img.Reference = await _fileService.SaveFile(img.Id, galleryId, file, file.Name);
                    }
                });

                _writeModel.Save(gallery.MapToEntity());

                return await Task.FromResult(Ok());
            }
            catch (Exception ex)
            {
                return BadRequest("Gallery does not exist");
            }
            
        }
    }
}