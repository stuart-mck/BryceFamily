using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BryceFamily.Web.MVC.Controllers
{
    public class ResourcesController : Controller
    {
        public IActionResult Thumbnail (Guid resourceid)
        {
            return NotFound();
        }

        public IActionResult Image(Guid resourceid)
        {
            return null;
        }

        public IActionResult PDF(Guid resourceid)
        {
            return null;
        }
    }
}