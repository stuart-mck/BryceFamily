using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BryceFamily.Web.MVC.Controllers
{
    public class VideoController : BaseController
    {
        public VideoController():base("Video", "video")
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}