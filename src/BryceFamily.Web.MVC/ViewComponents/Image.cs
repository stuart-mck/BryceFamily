using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Repo.Core.Read.ImageReference;
using System.Threading;
using BryceFamily.Web.MVC.Models;

namespace BryceFamily.Web.MVC.ViewComponents
{
    public class Image : ViewComponent
    {
        private readonly IImageReferenceReadRepository _imageReferenceReadRepository;

        public Image(IImageReferenceReadRepository imageReferenceReadRepository)
        {
            _imageReferenceReadRepository = imageReferenceReadRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid imageId, Guid galleryId)
        {
            var cancellationToken = CancellationToken.None;
            var img = await _imageReferenceReadRepository.Load(imageId, galleryId, cancellationToken);
            return View(new ImageViewModel()
            {
                GalleryId = galleryId,
                ImageId = imageId
            }.FullSizeLink);

        } 
    }
}
