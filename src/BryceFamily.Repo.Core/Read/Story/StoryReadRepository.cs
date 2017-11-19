using Amazon.DynamoDBv2.DataModel;
using BryceFamily.Repo.Core.AWS;
using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.Story
{
    public class StoryReadRepository : IStoryReadRepository
    {
        private readonly IAWSClientFactory _clientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;

        public StoryReadRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _clientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }


        public async Task<List<StoryIndex>> GetStoryIndexes(CancellationToken cancellationToken)
        {
            var dbContext = _clientFactory.GetDynamoDBContext();

            var dynamoOperationContext = new DynamoDBOperationConfig()
            {
                TableNamePrefix = "familybryce.",
                IndexName = "ID-index"
            };

            var results = await dbContext.ScanAsync<StoryIndex>(new List<ScanCondition>(), dynamoOperationContext).GetRemainingAsync(cancellationToken);

            return results;
        }

        public async Task<List<StoryContent>> GetStories(CancellationToken cancellationToken)
        {
            var dbContext = _clientFactory.GetDynamoDBContext();

            var results = await dbContext.ScanAsync<StoryContent>(new List<ScanCondition>(), _dynamoDBOperationConfig).GetRemainingAsync(cancellationToken);

            return results;
        }


        public async Task<StoryContent> Load(Guid id, CancellationToken cancellationToken)
        {
            var dbContext = _clientFactory.GetDynamoDBContext();
            return await dbContext.LoadAsync<StoryContent>(id, _dynamoDBOperationConfig, cancellationToken);
        }
    }
}
