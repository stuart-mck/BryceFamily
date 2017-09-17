using System;

namespace BryceFamily.Web.MVC.Models
{
    public class FamilyEvent
    {
        public Guid EntityId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string Details { get; set; }
        public string LocationTitle { get; set; }
        public eventType EventType { get; set; }
        public eventStatus EventStatus { get; set; }
        public string OrganiserName { get; set; }
        public string OrganiserContact { get; set; }
        public string OrganiserEmail { get; set; }
    }
}