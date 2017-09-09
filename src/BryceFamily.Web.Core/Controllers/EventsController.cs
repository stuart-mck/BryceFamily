using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.Core.Models;

namespace BryceFamily.Web.Core.Controllers
{
    [Route("api/[Controller]")]
    public class EventsController : Controller
    {
        public EventsController()
        {

        }

        [HttpGet("[action]")]
        public IEnumerable<Event> Events(bool activeOnly)
        {

            var dummyData = new List<Event>(){
                new Event(){
                    Dates = new EventDateTime(){
                        StartDate = new DateTime(2017, 12, 4, 14, 0, 0),
                        EndDate = new DateTime(2017, 12, 4, 14, 0, 0)
                    },
                    Details = "A gathering of the clan at Lake something or other",
                    Organiser = "Phillip Moore",
                    Title = "Bryce Family Reuninion",
                    EntityId = 1,
                    Location = new Location(){
                        Address1 = "Lake SOmething",
                        City = "Halls Gap",
                        Title = "Lake Something Camping Ground",
                    },
                    EventType = eventType.Gathering,
                    EventStatus = eventStatus.Pending
                },
                new Event(){
                    Dates = new EventDateTime(){
                        StartDate = new DateTime(2018, 3, 2, 14, 0, 0),
                        EndDate = new DateTime(2017, 3, 6, 14, 0, 0)
                    },
                    Details = "A gathering of the clan at Halls Gap",
                    Organiser = "Michael Moore",
                    Title = "Another Bryce Family Reuninion !",
                    EntityId = 1,
                    Location = new Location(){
                        Address1 = "THe Camping Ground",
                        City = "Halls Gap",
                        Title = "Lake Something Camping Ground",
                    },
                    EventType = eventType.Gathering,
                    EventStatus = eventStatus.Pending
                }
            };

            return dummyData;
        }
    }
}