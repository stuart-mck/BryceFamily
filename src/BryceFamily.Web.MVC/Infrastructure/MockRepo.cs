using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class MockRepo<TEntity, TId> : IReadModel<FamilyEvent, Guid>, IWriteModel<FamilyEvent, Guid>
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

        public Guid Save(FamilyEvent entity)
        {
            _familyEvents.Add(entity);
            return entity.ID;
        }

        public void Delete(Guid entityId)
        {
            var itemToRemove = _familyEvents.FirstOrDefault(t => t.ID == entityId);

            if (itemToRemove != null)
                _familyEvents.Remove(itemToRemove);

        }
    }
}
