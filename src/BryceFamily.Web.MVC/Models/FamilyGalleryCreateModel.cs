using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BryceFamily.Web.MVC.Models
{
    public class FamilyGalleryCreateModel
    {
        private readonly IReadOnlyList<FamilyClan> _clans;
        private readonly IReadOnlyList<FamilyEvent> _events;

        public FamilyGalleryCreateModel()
        {
            _clans = new List<FamilyClan>();
            _events = new List<FamilyEvent>();
            GalleryDate = DateTime.Today;

        }

        public FamilyGalleryCreateModel(List<FamilyClan> clans, List<FamilyEvent> events)
        {
            IComparer<FamilyClan> clanComparer = new FamilyComparer();
            clans.Sort(clanComparer);
            _clans = clans;

            IComparer<FamilyEvent> eventComparer = new EventComparer();
            events.Sort(eventComparer);
            _events = events;
        }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int FamilyId { get; set; }

        public Guid? EventId { get; set; }

        public DateTime GalleryDate { get; set; }

        public IReadOnlyList<FamilyClan> Families => _clans;

        public IReadOnlyList<FamilyEvent> Events => _events;
    }
}
