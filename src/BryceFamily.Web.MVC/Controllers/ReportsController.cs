using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Web.MVC.Models;
using BryceFamily.Web.MVC.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;

namespace BryceFamily.Web.MVC.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly ClanAndPeopleService _clanAndPeopleService;

        public ReportsController(ClanAndPeopleService clanAndPeopleService):base("Reports", "reports")
        {
            _clanAndPeopleService = clanAndPeopleService;
        }

        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult Vale()
        {
            return View(_clanAndPeopleService.People.Where(t => t.DateOfDeath.HasValue));
        }

        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult FamilyUpdates()
        {
            var updates = from clan in _clanAndPeopleService.People
                          where clan.ClanId.HasValue
                          group clan by clan.ClanId into g
                          select new FamilyUpdateSummary()
                          {
                              FamilyName = _clanAndPeopleService.Clans.First(cx => cx.Id == g.Key).FormattedName,
                              ClanId = g.Key.Value,
                              LastUpdate = g.Max(t => t.LastUpdated)
                          };
            
            return View(updates);
        }

        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult UpdatesByDate(DateTime fromDate)
        {
            var updates = _clanAndPeopleService.People.Where(d => d.LastUpdated > fromDate);
            return View(updates);
        }

        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult FamilyCountByState()
        {
            
            var updates = from state in _clanAndPeopleService.People
                            group state by state.State into g
                            select new FamilyStateSummary()
                            {
                                State = g.Key,
                                Count = g.Count()
                            };
            return View(updates);
            
        }

        [Authorize(Roles = RoleNameConstants.AllRoles)]
        public IActionResult OccupationSummary()
        {
            var people = from occupation in _clanAndPeopleService.People
                         where !string.IsNullOrEmpty(occupation.Occupation)
                         group occupation by occupation.Occupation into g
                         select new OccupationSummary()
                         {
                             Occupation = g.Key,
                             Count = g.Count()
                         };
            return View(people);

        }

    }
}