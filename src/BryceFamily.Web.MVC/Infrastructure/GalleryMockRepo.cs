using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class GalleryMockRepo<TEntity, TId> : IReadModel<Gallery, Guid>, IWriteModel<Gallery, Guid>
    {
        public GalleryMockRepo(List<Gallery> gallery)
        {
            _gallery = gallery;
        }

        private readonly List<Gallery> _gallery;

        public async Task<IQueryable<Gallery>> AsQueryable()
        {
            return await Task.FromResult(_gallery.AsQueryable());
        }

        public Task<List<Gallery>> GetByQuery(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<Gallery> Load(Guid id, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_gallery.First(t => t.ID == id));
        }

        public Guid Save(Gallery entity)
        {
            _gallery.Add(entity);
            return entity.ID;
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }
    }
}
