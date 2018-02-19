using System;
using System.Collections.Generic;

namespace BryceFamily.Web.MVC.Models
{
    public class FileUploadModel
    {
        public Guid GalleryId { get; set; }
        public string GalleryName { get; set; }

        public IEnumerable<Person> ClanMembers { get; set; }

        public int FamilyImageId { get; set; }
    }
}