using System;
using System.Threading;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Write.Query;
using BryceFamily.Repo.Core.AWS;
using Amazon.DynamoDBv2.DataModel;

namespace BryceFamily.Repo.Core.Write.ImageReference
{
    public class ImageReferenceWriteRepository<TEntity, TId> : IWriteRepository<Model.ImageReference, Guid>
    {
        private readonly IAWSClientFactory _clientFactory;
        private DynamoDBOperationConfig _dynamoDBOperationConfig;

        public ImageReferenceWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _clientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }


        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<Model.ImageReference> FindByQuery(IQueryParameter repository)
        {
            throw new NotImplementedException();
        }

        public async Task Save(Model.ImageReference entity, CancellationToken cancellationToken)
        {
            var dynamoContext = _clientFactory.GetDynamoDBContext();
            if (entity.ImageID == Guid.Empty)
                entity.ImageID = Guid.NewGuid();

            await dynamoContext.SaveAsync(entity, _dynamoDBOperationConfig, cancellationToken);
        }
    }
}
