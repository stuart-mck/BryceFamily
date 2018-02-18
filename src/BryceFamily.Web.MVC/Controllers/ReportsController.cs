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

        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult Index()
        {
            return View();
        }


        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult Vale()
        {
            return View(_clanAndPeopleService.People.Where(t => t.DateOfDeath.HasValue));
        }

        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
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

        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult UpdatesByDate(DateTime dateTime)
        {
            var updates = _clanAndPeopleService.People.Where(d => d.LastUpdated > dateTime);
            return View(updates);
        }

        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
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

    }
}