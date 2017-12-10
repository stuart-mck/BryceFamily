using System;
using System.Collections.Generic;
using System.Linq;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Read.FamilyEvents;
using System.Threading;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Read.ImageReference;
using BryceFamily.Web.MVC.Infrastructure;

namespace BryceFamily.Web.MVC.Models
{
    public class Gallery
    {
        public string Title { get; set; }
        public FamilyEvent FamilyEvent { get; set; }
        public string Owner { get; set; }
        public Guid? OwnerId { get; set; }
        public string Summary { get; set; }
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }

        public string Family { get; set; }


        public List<ImageReferenceModel> ImageReferences { get; private set; }

        public static async Task<Gallery> Map(Repo.Core.Model.Gallery sourceGallery, ClanAndPeopleService clanAndFamilyService, IFamilyEventReadRepository familyEventReadModel,  IImageReferenceReadRepository imageReferenceReadRepository, CancellationToken cancellationToken)
        {
            var familyName = sourceGallery.FamilyId.HasValue ?
                                $"{clanAndFamilyService.Clans.FirstOrDefault(c => c.Id == sourceGallery.FamilyId)?.FamilyName}, {clanAndFamilyService.Clans.FirstOrDefault(c => c.Id == sourceGallery.FamilyId)?.Family}"
                                : string.Empty;
            return await Task.FromResult(new Gallery() {
                Title = sourceGallery.Name,
                FamilyEvent = FamilyEvent.Map(await familyEventReadModel.Load(sourceGallery.FamilyEvent, cancellationToken)),
                Owner = sourceGallery.Owner.ToString(),
                OwnerId = sourceGallery?.Owner,
                Summary = sourceGallery.Summary,
                Id = sourceGallery.ID,
                DateCreated = sourceGallery.DateCreated,
                ImageReferences = MapImageReferences(await imageReferenceReadRepository.LoadByGallery(sourceGallery.ID, cancellationToken)),
                Family = familyName
            }
            );
        }

        private static List<ImageReferenceModel> MapImageReferences(IEnumerable<ImageReference> imageReferences)
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
                Summary = this.Summary
               
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
