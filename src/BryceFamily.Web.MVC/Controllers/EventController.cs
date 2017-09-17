using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;
using BryceFamily.Repo.Core.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IReadModel<Repo.Core.Model.FamilyEvent, Guid> _readmodel;
        private readonly IWriteModel<Repo.Core.Model.FamilyEvent, Guid> _writeModel;

        public EventController(IReadModel<Repo.Core.Model.FamilyEvent, Guid> readmodel, IWriteModel<Repo.Core.Model.FamilyEvent, Guid> writeModel)
        {
            _readmodel = readmodel;
            _writeModel = writeModel;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var events = (await _readmodel.AsQueryable()).ToList();
            
            return View(events.Select(FamilyEvent.Map));
        }

        public async Task<IActionResult> Detail(Guid eventId)
        {
            var familyEvent = await _readmodel.Load(eventId, new System.Threading.CancellationToken());
            return View(FamilyEvent.Map(familyEvent));

        }

        [HttpGet]
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
        public IActionResult NewEvent(FamilyEvent familyEventPost)
        {
            if (ModelState.IsValid)
            {
                _writeModel.Save(familyEventPost.MapToEntity());
                return RedirectToAction("Index");
            }
            return BadRequest();
            
        }

        //private IEnumerable<FamilyEvent> GetMocks()
        //{
        //    var dummyData = new List<FamilyEvent>(){
        //        new FamilyEvent(){
        //                StartDate = new DateTime(2017, 12, 4, 14, 0, 0),
        //                EndDate = new DateTime(2017, 12, 4, 14, 0, 0),
        //            Details = "A gathering of the clan at Lake something or other",
        //            OrganiserName = "Phillip Moore",
        //            Title = "Bryce Family Reuninion",
        //            EntityId = Guid.NewGuid(),
        //                Address1 = "Lake SOmething",
        //                City = "Halls Gap",
        //                LocationTitle = "Lake Something Camping Ground",
        //            EventType = eventType.Gathering,
        //            EventStatus = eventStatus.Pending
        //        },
        //        new FamilyEvent(){
        //            StartDate = new DateTime(2018, 3, 2, 14, 0, 0),
        //                EndDate = new DateTime(2017, 3, 6, 14, 0, 0),
        //            Details = "A gathering of the clan at Halls Gap",
        //            OrganiserName = "Michael Moore",
        //            Title = "Another Bryce Family Reuninion !",
        //            EntityId = Guid.NewGuid(),
        //                Address1 = "The Camping Ground",
        //                City = "Halls Gap",
        //                LocationTitle = "Lake Something Camping Ground",
        //            EventType = eventType.Gathering,
        //            EventStatus = eventStatus.Pending
        //        }
        //    };

        //    return dummyData;
        //}
    }
}
        