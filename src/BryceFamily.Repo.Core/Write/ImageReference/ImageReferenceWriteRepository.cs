using System;
using System.Threading;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Write.Query;
using BryceFamily.Repo.Core.AWS;
using Amazon.DynamoDBv2.DataModel;
using BryceFamily.Repo.Core.Model;

namespace BryceFamily.Repo.Core.Write.ImageReference
{
    public class ImageReferenceWriteRepository<TEntity, TId> : IWriteRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        private readonly IAWSClientFactory _clientFactory;
        private DynamoDBOperationConfig _dynamoDBOperationConfig;

        public ImageReferenceWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
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
            entity.LastUpdated = DateTime.Now;
            await dynamoContext.SaveAsync(entity, _dynamoDBOperationConfig, cancellationToken);
        }
    }
}
