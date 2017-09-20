using System;
using System.Collections.Generic;
using System.Linq;
using BryceFamily.Repo.Core.Model;

namespace BryceFamily.Web.MVC.Models
{
    public class Gallery
    {
        public string Title { get; set; }
        public FamilyEvent FamilyEvent { get; set; }
        public string Owner { get; set; }
        public Guid OwnerId { get; set; }
        public string Summary { get; set; }
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }

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

        public Repo.Core.Model.Gallery MapToEntity()
        {
            return new Repo.Core.Model.Gallery()
            {
                ID = this.Id,
                DateCreated = this.DateCreated,
                Name = this.Title,
                Summary = this.Summary,
                ImageReferences = BuildImageReferences()
               
            };
        }

        private List<ImageReference> BuildImageReferences()
        {
            if (ImageReferences == null || !ImageReferences.Any())
                return new List<ImageReference>();

           return ImageReferences.Select(ir => ir.MapToEntity()).ToList();
        }
    }
}
