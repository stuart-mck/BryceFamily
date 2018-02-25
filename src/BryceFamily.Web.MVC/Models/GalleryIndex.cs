using BryceFamily.Repo.Core.Read.FamilyEvents;
using BryceFamily.Web.MVC.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models
{
    public class GalleryIndex
    {
        public string Title { get; set; }
        public FamilyEvent FamilyEvent { get; set; }
        public string Owner { get; set; }
        public int OwnerId { get; set; }
        public string Summary { get; set; }
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime GalleryDate { get; set; }

        public string Family { get; set; }

        public FamilyClan Clan { get; set; }


        public bool DefaultFamilyEventGallery { get; set; }


        public static async Task<GalleryIndex> Map(Repo.Core.Model.Gallery sourceGallery, ClanAndPeopleService clanAndFamilyService, IFamilyEventReadRepository familyEventReadModel, CancellationToken cancellationToken)
        {
            var familyName = sourceGallery.FamilyId.HasValue ?
                                $"{clanAndFamilyService.Clans.FirstOrDefault(c => c.Id == sourceGallery.FamilyId)?.FamilyName}, {clanAndFamilyService.Clans.FirstOrDefault(c => c.Id == sourceGallery.FamilyId)?.Family}"
                                : string.Empty;
            return await Task.FromResult(new GalleryIndex()
            {
                Title = sourceGallery.Name,
                FamilyEvent = FamilyEvent.Map(await familyEventReadModel.Load(sourceGallery.FamilyEvent, cancellationToken)),
                Owner = sourceGallery.Owner.ToString(),
                OwnerId = sourceGallery.Owner,
                Summary = sourceGallery.Summary,
                Id = sourceGallery.ID,
                DateCreated = sourceGallery.DateCreated,
                Family = familyName,
                DefaultFamilyEventGallery = sourceGallery.DefaultFamilyEventGallery,
                GalleryDate = sourceGallery.GalleryDate,
                Clan = clanAndFamilyService.Clans.FirstOrDefault(t => t.Id == sourceGallery.FamilyId)
            }
            );
        }
    }
}
