using System;
using BryceFamily.Repo.Core.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BryceFamily.Web.MVC.Models
{
    public class FamilyEvent
    {
    
        public Guid EntityId { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
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
        public Guid GalleryId { get; set; }

        public Guid ImageReference { get; set; }

        public bool FrontPage { get; set; }


        public List<string> GalleryImageList { get; }

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

        public string ImagePath { get; private set; }

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
                OrganiserName = OrganiserName,
                FrontPage = FrontPage
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
            return MapWithImageReference(sourceFamilyEvent, Guid.Empty, Guid.Empty, string.Empty);
        }

        public static FamilyEvent MapWithImageReference(Repo.Core.Model.FamilyEvent sourceFamilyEvent, Guid imageReference, Guid  galleryId, string imageTitle)
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
                OrganiserContact = sourceFamilyEvent.OrganiserEmail,
                ImageReference = imageReference,
                ImagePath = $"{imageReference}/{imageTitle}",
                GalleryId = galleryId,
                FrontPage = sourceFamilyEvent.FrontPage
                

            };
        }
    }
}