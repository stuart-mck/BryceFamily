using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class MockPeopleService<TEntity, TId> : IReadModel<Person, Guid>, IWriteModel<Person, Guid>
    {
        private List<Person> _people;

        public async Task<IQueryable<Person>> AsQueryable()
        {
            return await Task.FromResult(_people.AsQueryable());
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Person>> GetByQuery(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> Load(Guid id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_people.First(p => p.ID == id));
        }

        public Guid Save(Person entity)
        {
            var current = _people.FirstOrDefault(p => p.ID == entity.ID);
            if (current != null)
                _people.Remove(current);
            _people.Add(entity);
            return entity.ID;
        }
    }
}
