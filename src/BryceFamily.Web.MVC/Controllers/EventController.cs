﻿using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Repo.Core.Read.FamilyEvents;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using BryceFamily.Web.MVC.Infrastructure.Authentication;

namespace BryceFamily.Web.MVC.Models
{
    public class EventController : Controller
    {
        private readonly IFamilyEventReadRepository _readmodel;
        private readonly IWriteRepository<Repo.Core.Model.FamilyEvent, Guid> _writeModel;

        public EventController(IFamilyEventReadRepository readmodel, IWriteRepository<Repo.Core.Model.FamilyEvent, Guid> writeModel)
        {
            _readmodel = readmodel;
            _writeModel = writeModel;
        }

        [AllowAnonymous]
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var events = (await _readmodel.GetAllEvents( new CancellationToken())).ToList();
            
            return View(events.Select(FamilyEvent.Map));
        }

        public async Task<IActionResult> Detail(Guid eventId)
        {
            var familyEvent = await _readmodel.Load(eventId, new CancellationToken());
            return View(FamilyEvent.Map(familyEvent));

        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult NewEvent()
        {
            return View("EditEvent", new FamilyEvent()
            {
                EntityId = Guid.NewGuid(),
                EventStatus = eventStatus.Pending,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            });
        }

        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public IActionResult NewEvent(FamilyEvent familyEventPost)
        {
            if (ModelState.IsValid)
            {
                _writeModel.Save(familyEventPost.MapToEntity(), new System.Threading.CancellationToken());
                return RedirectToAction("Index");
            }
            return BadRequest();
            
        }

    }
}
        