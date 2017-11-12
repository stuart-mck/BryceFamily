using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;


namespace BryceFamily.Web.MVC.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Story()
        {
            return View(new StoryWriteModel());
        }

        [HttpPost]
        public IActionResult Story(StoryWriteModel storyWriteModel)
        {
            return View();
        }
    }
}