using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;
using BryceFamily.Web.MVC.Infrastructure;

namespace BryceFamily.Web.MVC.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ClanAndPeopleService _clanService;

        public HistoryController(ClanAndPeopleService clanService )
        {
            this._clanService = clanService;
        }

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

        public IActionResult Tree (int top = 0)
        {

        }
    }
}