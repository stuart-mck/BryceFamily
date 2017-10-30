using Amazon.DynamoDBv2.DataModel;
using BryceFamily.Repo.Core.AWS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.Gallery
{
    public class GalleryReadRepository : IGalleryReadRepository
    {
        private readonly IAWSClientFactory _awsClientFactory;
        private readonly DynamoDBOperationConfig _operationConfig;

        public GalleryReadRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig operationConfig)
        {
            _awsClientFactory = awsClientFactory;
            _operationConfig = operationConfig;
        }

        public async Task<List<Model.Gallery>> LoadAll(CancellationToken cancellationToken)
        {
            var dynamoContext = _awsClientFactory.GetDynamoDBContext();

            return await dynamoContext.ScanAsync<Model.Gallery>(new List<ScanCondition>(), _operationConfig).GetRemainingAsync(cancellationToken);
        }

        public async Task<Model.Gallery> Load(Guid id, CancellationToken cancellationToken)
        {
            var dynamoContext = _awsClientFactory.GetDynamoDBContext();
            return await dynamoContext.LoadAsync<Model.Gallery>(id, _operationConfig, cancellationToken);
        }

    }
}
