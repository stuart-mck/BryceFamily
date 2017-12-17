using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Write.Query;

namespace BryceFamily.Repo.Core.Write
{
    public interface IWriteQuery<TEntity, TId> where TEntity : Entity<TId>
    {
        IQueryParameter QueryParameter { get; }
    }
}
