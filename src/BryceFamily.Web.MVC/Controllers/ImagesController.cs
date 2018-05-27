using BryceFamily.Repo.Core.Files;
using BryceFamily.Repo.Core.Read.ImageReference;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IImageReferenceReadRepository _imageReferenceReadRepository;
        private readonly IFileService _fileService;

        public ImagesController(IImageReferenceReadRepository imageReferenceReadRepository,
                                 IFileService fileService)
        {
            _imageReferenceReadRepository = imageReferenceReadRepository;
            _fileService = fileService;
        }

        [Route("Images/View/{galleryId}/{imageId}")]
        public async Task<IActionResult> View(Guid galleryId, Guid imageId)
        {
            var cancellationToken = CancellationToken.None;
            var imageInfo = await _imageReferenceReadRepository.Load(galleryId, imageId, cancellationToken);
            if (imageInfo == null)
                return NotFound();

            var imageBytes = await _fileService.GetFile(galleryId, imageId);

            if (imageBytes == null)
                return NotFound();

            return File(imageBytes, imageInfo.MimeType);
        }
    }
}
