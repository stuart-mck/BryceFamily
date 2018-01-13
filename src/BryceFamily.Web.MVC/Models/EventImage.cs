using System;

namespace BryceFamily.Web.MVC.Models
{
    public class EventImage
    {

        public EventImage(Guid iD)
        {
            EventId = iD;
        }

        public Guid EventId { get; set; }

        public string EventName { get; set; }

    }
}
