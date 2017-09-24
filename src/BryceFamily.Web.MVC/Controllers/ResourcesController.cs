using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Repo.Core.Files;
using BryceFamily.Repo.Core.Repository;
using BryceFamily.Repo.Core.Model;
using System.Threading;

namespace BryceFamily.Web.MVC.Controllers
{
    [Route("ImageServer")]
    public class ResourcesController : Controller
    {
        private IFileService _fileService;
        private readonly IReadModel<Gallery, Guid> _galleryReadModel;

        public ResourcesController(IFileService fileService, IReadModel<Gallery, Guid> galleryReadModel)
        {
            _fileService = fileService;
            _galleryReadModel = galleryReadModel;
        }
        
        [HttpGet, Route("T")]
        public async Task<IActionResult> Thumbnail (Guid galleryId, Guid imageId)
        {
            try
            {
                var gallery = await _galleryReadModel.Load(galleryId, CancellationToken.None);

                var resource = gallery.ImageReferences.FirstOrDefault(t => t.ID == imageId);
                if (resource == null)
                    return NotFound();

                return File(await _fileService.GetFileResized(imageId, gallery.ID, 150d), resource.MimeType);
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
                var gallery = await _galleryReadModel.Load(galleryId, CancellationToken.None);

                var resource = gallery.ImageReferences.FirstOrDefault(t => t.ID == imageId);
                if (resource == null)
                    return NotFound();

                return File(await _fileService.GetFileResized(imageId, galleryId, 800d), resource.MimeType);
            }
            catch(Exception ex)
            {
                return NotFound();
            }
        }

        public IActionResult PDF(Guid resourceid)
        {
            return null;
        }
    }
}