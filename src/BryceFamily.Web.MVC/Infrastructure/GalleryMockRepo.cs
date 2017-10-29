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
    public class GalleryMockRepo<TEntity, TId> : IReadModel<Gallery, Guid>, IWriteRepository<Gallery, Guid>
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

        public async Task Save(Gallery entity, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                var existingGallery = _gallery.FirstOrDefault(g => g.ID == entity.ID);
                if (existingGallery != null)
                    _gallery.Remove(existingGallery);

                _gallery.Add(entity);
            });
            
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<Gallery> FindByQuery(IQueryParameter repository)
        {
            throw new NotImplementedException();
        }
    }
}
