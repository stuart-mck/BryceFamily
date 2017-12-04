using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BryceFamily.Web.MVC.Models
{
    public class FamilyGalleryCreateModel
    {
        private IReadOnlyList<FamilyClan> _clans;

        public FamilyGalleryCreateModel()
        {
            _clans = new List<FamilyClan>();
        }

        public FamilyGalleryCreateModel(IReadOnlyList<FamilyClan> clans)
        {
            _clans = clans;
        }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int FamilyId { get; set; }

        public IReadOnlyList<FamilyClan> Families => _clans;
    }
}
