using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Write.Query;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Write
{
    public interface IWriteRepository<TEntity, TId> where TEntity : Entity
    {
        Task Save(TEntity entity, CancellationToken cancellationToken);

        void Delete(TId entityId);

       Task<TEntity> FindByQuery(IQueryParameter repository, CancellationToken cancellationToken);
    }
}
