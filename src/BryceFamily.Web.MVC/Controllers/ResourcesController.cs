﻿using BryceFamily.Repo.Core.Files;
using BryceFamily.Repo.Core.Read.Gallery;
using BryceFamily.Repo.Core.Read.ImageReference;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Controllers
{
    [Route("ImageServer")]
    public class ResourcesController : BaseController
    {
        private IFileService _fileService;
        private readonly IGalleryReadRepository _galleryReadModel;
        private readonly IImageReferenceReadRepository _imageReferenceReadRepository;

        public ResourcesController(IFileService fileService, IGalleryReadRepository galleryReadModel, IImageReferenceReadRepository imageReferenceReadRepository):base("Resources", "Resources")
        {
            _fileService = fileService;
            _galleryReadModel = galleryReadModel;
            _imageReferenceReadRepository = imageReferenceReadRepository;
        }
        
        [HttpGet, Route("T")]
        public async Task<IActionResult> Thumbnail (Guid galleryId, Guid imageId)
        {
            try
            {
                var cancellationToken = CancellationToken.None;

                var resource = await _imageReferenceReadRepository.Load(galleryId, imageId, cancellationToken);
                if (resource == null)
                    return NotFound();

                return File(await _fileService.GetFileResized(imageId, galleryId, 150d), resource.MimeType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        [HttpGet, Route("I")]
        public async Task<IActionResult> Image(Guid galleryId,  Guid imageId)
        {
            try
            {

                var resource = await _imageReferenceReadRepository.Load(galleryId, imageId, CancellationToken.None);
                if (resource == null)
                    return NotFound();

                return File(await _fileService.GetFileResized(imageId, galleryId, 800d), resource.MimeType);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        public IActionResult PDF(Guid resourceid)
        {
            return null;
        }
    }
}