using Amazon.DynamoDBv2.DataModel;
using BryceFamily.Repo.Core.AWS;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.ImageReference
{
    public class ImageReferenceReadRepository : IImageReferenceReadRepository
    {
        private readonly IAWSClientFactory _awsClientFactory;
        private readonly DynamoDBOperationConfig _operationConfig;

        public ImageReferenceReadRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig operationConfig)
        {
            _awsClientFactory = awsClientFactory;
            _operationConfig = operationConfig;
        }

        public async Task<Model.ImageReference> Load(Guid imageId, Guid galleryId, CancellationToken cancellationToken)
        {
            var dynamoContext = _awsClientFactory.GetDynamoDBContext();
            return await dynamoContext.LoadAsync<Model.ImageReference>(galleryId, imageId, _operationConfig, cancellationToken);
            
        }

        public async Task<IEnumerable<Model.ImageReference>> LoadByGallery(Guid galleryId, CancellationToken cancellationToken)
        {
            var dynamoContext = _awsClientFactory.GetDynamoDBContext();
            return await dynamoContext.QueryAsync<Model.ImageReference>(galleryId, _operationConfig).GetRemainingAsync(cancellationToken);
        }
    }
}
