using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace BryceFamily.Repo.Core.AWS
{
    public class AWSClientFactory : IAWSClientFactory
    {
        private AmazonDynamoDBClient _client = new AmazonDynamoDBClient();

        private IAmazonDynamoDB GetDynamoDbClient()
        {
            if (_client == null)
                _client = new AmazonDynamoDBClient(RegionEndpoint.APSoutheast2);
            return _client;
        }

        public IDynamoDBContext GetDynamoDBContext()
        {
            return new DynamoDBContext(GetDynamoDbClient());
        }
    }
}
