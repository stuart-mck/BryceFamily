using BryceFamily.Repo.Core.Model;
using System;
using BryceFamily.Repo.Core.Write.Query;
using System.Threading;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.AWS;
using Amazon.DynamoDBv2.DataModel;

namespace BryceFamily.Repo.Core.Write.Story
{
    public class StoryWriteRepository<TEntity, TId> : IWriteRepository<StoryContent, Guid>
    {
        private readonly IAWSClientFactory _clientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;

        public StoryWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _clientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<StoryContent> FindByQuery(IQueryParameter query, CancellationToken cancellationToken)
        {
            var storyId = ((StoryQuery)query).StoryId;
            var dbContext = _clientFactory.GetDynamoDBContext();
            return await dbContext.LoadAsync<StoryContent>(storyId, _dynamoDBOperationConfig, cancellationToken);
        }

        public async Task Save(StoryContent entity, CancellationToken cancellationToken)
        {
            if (entity.ID == Guid.Empty)
                entity.ID = Guid.NewGuid();
            var dbContext = _clientFactory.GetDynamoDBContext();
            await dbContext.SaveAsync(entity, _dynamoDBOperationConfig, cancellationToken);
        }
    }
}
