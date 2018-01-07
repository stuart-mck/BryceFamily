using System;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("imageReference")]
    public class ImageReference : Entity<Guid>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageLocation { get; set; }

        public string MimeType { get; set; }

        public bool DefaultGalleryImage { get; set; }

        public IEnumerable<ImageAssociation> ImageAssociations { get; set; }

        [DynamoDBRangeKey]
        public Guid ImageID { get; set; }
    }
}
