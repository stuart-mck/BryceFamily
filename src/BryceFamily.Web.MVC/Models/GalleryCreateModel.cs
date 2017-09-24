using System;

namespace BryceFamily.Web.MVC.Models
{
    public class GalleryCreateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid FamilyEventId { get; set; }
    }
}
