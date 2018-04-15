using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.ImageReference
{
    public interface IImageReferenceReadRepository
    {
        Task<Model.ImageReference> Load(Guid imageReferenceId, CancellationToken cancellationToken);
        Task<Model.ImageReference> Load(Guid imageId, Guid galleryId, CancellationToken cancellationToken);
        Task<IEnumerable<Model.ImageReference>> LoadByGallery(Guid galleryId, CancellationToken cancellationToken);
    }
}
