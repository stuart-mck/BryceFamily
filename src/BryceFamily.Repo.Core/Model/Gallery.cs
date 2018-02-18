using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("gallery")]
    public class Gallery : Entity<Guid>
    {
        public string Name { get; set; }

        public int Owner { get; set; }

        public string Summary { get; set; }

        public Guid FamilyEvent { get; set; }

        public int? FamilyId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime GalleryDate { get; set; }

        public bool DefaultFamilyEventGallery { get; set; }


    }
}
