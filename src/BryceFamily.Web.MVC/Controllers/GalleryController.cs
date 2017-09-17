using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Repo.Core.Repository;
using BryceFamily.Web.MVC.Models;

namespace BryceFamily.Web.MVC.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IReadModel<Repo.Core.Model.Gallery, Guid> _readModel;

        public GalleryController(IReadModel<Repo.Core.Model.Gallery, Guid> readModel)
        {
            _readModel = readModel;
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            var gallery = await _readModel.Load(id, new System.Threading.CancellationToken());

            return View(Gallery.Map(gallery));
        }

        public async Task<IActionResult> Index()
        {
            var galleries = (await _readModel.AsQueryable()).ToList();

            return View(galleries.Select(t => {
                return Gallery.Map(t);
                }));
            
        }
    }
}