using Microsoft.AspNetCore.Http;
using System;


namespace BryceFamily.Web.MVC.Models
{
    public class FamilyEventImage
    {
        public Guid FamilyEventId { get; set; }
        public Guid FamilyEventGalleryId { get; set; }

        public IFormFile DefaultImage { get; set; }

    }
}
