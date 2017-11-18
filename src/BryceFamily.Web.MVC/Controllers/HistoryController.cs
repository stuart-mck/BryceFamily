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


        public IActionResult Tree (Guid top)
        {
            Person startNode;
            if (top == null)
                startNode = _clanService.People.First(p => p.IsSpouse == false && p.Mother == null & p.Father == null);
            else
                startNode = _clanService.People.FirstOrDefault(p => p.Id == top);

            if (startNode == null)
                return BadRequest("Invalid Person reference");

                return View(startNode);
        }
    }
}