using System;

namespace BryceFamily.Web.MVC.Models
{
    public class EventImage
    {

        public EventImage(Guid eventId)
        {
            EventId = eventId;
        }

        public Guid EventId { get; set; }

        public string EventName { get; set; }

    }
}
