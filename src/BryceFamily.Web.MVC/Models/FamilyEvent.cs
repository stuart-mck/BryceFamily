using System;
using BryceFamily.Repo.Core.Model;

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

        public string FormattedStartDate
        {
            get
            {
                return StartDate.ToLongDateString();
            }
        }


        public string FormattedEndDate
        {
            get
            {
                return EndDate.ToLongDateString();
            }
        }

        public Repo.Core.Model.FamilyEvent MapToEntity()
        {
            return new Repo.Core.Model.FamilyEvent()
            {
                Details = Details,
                StartDate = StartDate,
                EndDate = EndDate,
                EventStatus = (EventStatus)this.EventStatus,
                EventType = (EventType)this.EventType,
                ID = EntityId,
                Location = MapLocation(),
                Title = Title,
                OrganiserContact = OrganiserContact,
                OrganiserEmail = OrganiserEmail,
                OrganiserName = OrganiserName
            };
        }

        private EventLocation MapLocation()
        {
            return new EventLocation()
            {
                Address1 = Address1,
                Address2 = Address2,
                City = City,
                PostCode = PostCode,
                State = State,
                Title = LocationTitle
            };
        }

        public static FamilyEvent Map(Repo.Core.Model.FamilyEvent sourceFamilyEvent)
        {
            if (sourceFamilyEvent == null)
                return null;
            return new FamilyEvent()
            {
                Address1 = sourceFamilyEvent.Location?.Address1,
                Address2 = sourceFamilyEvent.Location?.Address2,
                City = sourceFamilyEvent.Location?.City,
                PostCode = sourceFamilyEvent.Location?.PostCode,
                LocationTitle = sourceFamilyEvent.Location?.Title,
                EntityId = sourceFamilyEvent.ID,
                EventStatus = (eventStatus)sourceFamilyEvent.EventStatus,
                EventType = (eventType)sourceFamilyEvent.EventType,
                Details = sourceFamilyEvent.Details,
                Title = sourceFamilyEvent.Title,
                StartDate = sourceFamilyEvent.StartDate,
                EndDate = sourceFamilyEvent.EndDate,
                OrganiserName = sourceFamilyEvent.OrganiserName,
                OrganiserEmail = sourceFamilyEvent.OrganiserEmail,
                OrganiserContact = sourceFamilyEvent.OrganiserEmail

            };
        }
    }
}