using System;
using System.Collections.Generic;
using System.Linq;

namespace BryceFamily.Web.MVC.Models
{
    public class Gallery
    {
        public string Title { get; set; }
        public FamilyEvent FamilyEvent { get; set; }
        public string Owner { get; private set; }
        public Guid OwnerId { get; private set; }
        public string Summary { get; private set; }
        public Guid Id { get; private set; }
        public DateTime DateCreated { get; private set; }

        public List<ImageReferenceModel> ImageReferences { get; private set; }

        public static Gallery Map(Repo.Core.Model.Gallery sourceGallery)
        {
            return new Gallery()
            {
                Title = sourceGallery.Name,
                FamilyEvent = FamilyEvent.Map(sourceGallery.FamilyEvent),
                Owner = $"{sourceGallery.Owner?.FirstName} {sourceGallery.Owner?.LastName}",
                OwnerId = sourceGallery.Owner.ID,
                Summary = sourceGallery.Summary,
                Id = sourceGallery.ID,
                DateCreated = sourceGallery.DateCreated,
                ImageReferences = MapImageReferences(sourceGallery.ImageReferences)
            };
        }

        private static List<ImageReferenceModel> MapImageReferences(List<Repo.Core.Model.ImageReference> imageReferences)
        {
            var results = new List<ImageReferenceModel>();

            if (imageReferences != null)
                results.AddRange(imageReferences.Select(ImageReferenceModel.Map));

            return results;
        }
    }
}
