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
    public class ResourcesController : Controller
    {
        private IFileService _fileService;
        private readonly IReadModel<Gallery, Guid> _galleryReadModel;

        public ResourcesController(IFileService fileService, IReadModel<Gallery, Guid> galleryReadModel)
        {
            _fileService = fileService;
            _galleryReadModel = galleryReadModel;
        }
        
        [HttpGet]
        public IActionResult Thumbnail (Guid resourceid)
        {
            return NotFound();
        }

        [HttpGet, Route("{galleryId}/{resourceId}")]
        public async Task<IActionResult> Image(Guid galleryId,  Guid resourceId)
        {
            try
            {
                var gallery = await _galleryReadModel.Load(galleryId, CancellationToken.None);

                var resource = gallery.ImageReferences.FirstOrDefault(t => t.ID == resourceId);
                if (resource == null)
                    return NotFound();

                return File(await _fileService.GetFile(resourceId, galleryId), resource.MimeType);
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