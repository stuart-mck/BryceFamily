using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.SimpleEmail;

namespace BryceFamily.Repo.Core.AWS
{
    public class AWSClientFactory : IAWSClientFactory
    {
        private AmazonDynamoDBClient _client = new AmazonDynamoDBClient(RegionEndpoint.APSoutheast2);

        private IAmazonDynamoDB GetDynamoDbClient()
        {
            return _client;
        }

        public IDynamoDBContext GetDynamoDBContext()
        {
            return new DynamoDBContext(GetDynamoDbClient());
        }

        public IAmazonS3 GetS3Context()
        {
            return new AmazonS3Client(RegionEndpoint.APSoutheast2);
        }

        public IAmazonSimpleEmailService GetSesClient()
        {
            return new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1);
        }

    }
}

