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
    public class MockPeopleService<TEntity, TId> : IReadModel<Person, Guid>, IWriteRepository<Person, Guid>
    {
        private readonly List<Person> _people = new List<Person>();


        public async Task<IQueryable<Person>> AsQueryable()
        {
            return await Task.FromResult(_people.AsQueryable());
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<Person> FindByQuery(IQueryParameter repository)
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

        public async Task Save(Person entity, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var current = _people.FirstOrDefault(p => p.ID == entity.ID);
                if (current != null)
                    _people.Remove(current);
                _people.Add(entity);
            });
        }
    }
}
