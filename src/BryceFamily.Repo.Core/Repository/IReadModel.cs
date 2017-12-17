using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Repository
{
    public interface IReadModel<TEntity, TId> where TEntity : Entity<TId>
    {
        Task<TEntity> Load(TId id, CancellationToken cancellationToken);

        Task<List<TEntity>> GetByQuery(DateTime startDate, DateTime endDate);

        Task<IQueryable<TEntity>> AsQueryable();

    }
}
