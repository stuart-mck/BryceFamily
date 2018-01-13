using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.Gallery
{
    public interface IGalleryReadRepository
    {
        Task<List<Model.Gallery>> LoadAll(CancellationToken cancellationToken);

        Task<Model.Gallery> Load(Guid id, CancellationToken cancellationToken);

        Task<IEnumerable<Model.Gallery>> FindAllByFamilyEvent(Guid familyEventId, CancellationToken cancellationToken);
    }
}
