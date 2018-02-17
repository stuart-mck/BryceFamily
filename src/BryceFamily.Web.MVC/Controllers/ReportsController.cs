using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Web.MVC.Models;

namespace BryceFamily.Web.MVC.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly ClanAndPeopleService _clanAndPeopleService;

        public ReportsController(ClanAndPeopleService clanAndPeopleService):base("Reports", "reports")
        {
            _clanAndPeopleService = clanAndPeopleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Vale()
        {
            return View(_clanAndPeopleService.People.Where(t => t.DateOfDeath.HasValue));
        }

        public IActionResult FamilyUpdates()
        {
            var updates = from clan in _clanAndPeopleService.People
                          group clan by clan.ClanId into g
                          select new FamilyUpdateSummary()
                          {
                              FamilyName = _clanAndPeopleService.Clans.First(cx => cx.Id == g.Key).FormattedName,
                              ClanId = g.Key,
                              LastUpdate = g.Max(t => t.LastUpdated)
                          };
            
            return View(updates);
        }

    }
}