using BryceFamily.Repo.Core.Model;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Repository
{
    public interface IReadModel<TEntity, TId> where TEntity : Entity
    {
        Task<TEntity> Load(TId id, CancellationToken cancellationToken);

    }
}
