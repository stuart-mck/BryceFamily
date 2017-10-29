using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Repo.Core.Write.Query;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class MockRepo<TEntity, TId> : IReadModel<FamilyEvent, Guid>, IWriteRepository<FamilyEvent, Guid>
    {
        private readonly List<FamilyEvent> _familyEvents;

        public MockRepo(List<FamilyEvent> familyEvents)
        {
            _familyEvents = familyEvents;
        }

        public async Task<FamilyEvent> Load(Guid id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_familyEvents.First(fe => fe.ID == id));
        }

        public async Task<List<FamilyEvent>> GetByQuery(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_familyEvents.Where(t => t.StartDate >= startDate && t.EndDate < endDate).ToList());
        }

        public async Task Save(FamilyEvent entity, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                _familyEvents.Add(entity);
            });
        }

        public void Delete(Guid entityId)
        {
            var itemToRemove = _familyEvents.FirstOrDefault(t => t.ID == entityId);

            if (itemToRemove != null)
                _familyEvents.Remove(itemToRemove);

        }

        public async Task<IQueryable<FamilyEvent>> AsQueryable()
        {
            return await Task.FromResult(_familyEvents.AsQueryable());
        }

        public Task<FamilyEvent> FindByQuery(IQueryParameter repository)
        {
            throw new NotImplementedException();
        }
    }
}
