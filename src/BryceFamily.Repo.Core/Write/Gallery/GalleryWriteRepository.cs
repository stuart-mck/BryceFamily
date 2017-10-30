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
    public class GalleryWriteRepository<TEntity, TId> : IWriteRepository<Model.Gallery, Guid>
    {
        private readonly IAWSClientFactory _clientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;
        public GalleryWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _clientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<Model.Gallery> FindByQuery(IQueryParameter repository)
        {
            throw new NotImplementedException();
        }



        public async Task Save(Model.Gallery entity, CancellationToken cancellationToken)
        {
            var dynamoContext = _clientFactory.GetDynamoDBContext();
            if (entity.ID == Guid.Empty)
                entity.ID = Guid.NewGuid();
            await dynamoContext.SaveAsync(entity, _dynamoDBOperationConfig, cancellationToken);
        }
    }
}
