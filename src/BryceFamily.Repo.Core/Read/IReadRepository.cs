using BryceFamily.Repo.Core.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read
{
    public interface IReadRepository<TEntity, TID> where TEntity : Entity
    {
        Task<TEntity> Load(TID id, CancellationToken cancellationToken);

        Task<List<TEntity>> GetByQuery();

    }
}
