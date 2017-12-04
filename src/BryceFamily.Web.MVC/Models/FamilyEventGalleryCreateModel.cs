using System;
using System.Collections.Generic;

namespace BryceFamily.Web.MVC.Models
{
    public class FamilyEventGalleryCreateModel
    {
        private IEnumerable<FamilyEvent> _events;

        public FamilyEventGalleryCreateModel()
        {
            _events = new List<FamilyEvent>();
        }

        public FamilyEventGalleryCreateModel(IEnumerable<FamilyEvent> events)
        {
            _events = events;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid FamilyEventId { get; set; }

        public IEnumerable<FamilyEvent> FamilyEvents => _events;


    }
}
