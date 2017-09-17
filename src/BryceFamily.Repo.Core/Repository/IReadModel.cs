using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Repository
{
    public interface IReadModel<TEntity, TId> where TEntity : Entity
    {
        Task<TEntity> Load(TId id, CancellationToken cancellationToken);

        Task<List<FamilyEvent>> GetByQuery(DateTime startDate, DateTime endDate);

        Task<IQueryable<FamilyEvent>> AsQueryable();

    }
}
