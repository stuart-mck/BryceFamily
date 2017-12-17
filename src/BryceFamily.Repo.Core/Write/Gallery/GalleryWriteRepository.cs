using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Write.Query;
using BryceFamily.Repo.Core.AWS;
using Amazon.DynamoDBv2.DataModel;

namespace BryceFamily.Repo.Core.Write.Gallery
{
    public class GalleryWriteRepository<TEntity, TId> : IWriteRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        private readonly IAWSClientFactory _clientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;
        public GalleryWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _clientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }

        public void Delete(TId entityId)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindByQuery(IQueryParameter repository, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }



        public async Task Save(TEntity entity, CancellationToken cancellationToken)
        {
            var dynamoContext = _clientFactory.GetDynamoDBContext();
            //if (entity.ID == Guid.Empty)
            //    entity.ID = Guid.NewGuid();
            await dynamoContext.SaveAsync(entity, _dynamoDBOperationConfig, cancellationToken);
        }
    }
}
