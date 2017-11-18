using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;
using BryceFamily.Web.MVC.Infrastructure;
using System.Linq;
using System;

namespace BryceFamily.Web.MVC.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ClanAndPeopleService _clanService;

        public HistoryController(ClanAndPeopleService clanService )
        {
            _clanService = clanService;
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

        [Route("History/Tree/{id}")]
        public IActionResult Tree (Guid id)
        {
            Person startNode;
            if (id == Guid.Empty)
                startNode = _clanService.People.First(p => p.IsSpouse == false && p.Mother == null & p.Father == null);
            else
                startNode = _clanService.People.FirstOrDefault(p => p.Id == id);

            if (startNode == null)
                return BadRequest("Invalid Person reference");

            return View(startNode);
        }
    }
}