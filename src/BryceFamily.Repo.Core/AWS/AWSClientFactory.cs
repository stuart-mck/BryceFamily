using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;

namespace BryceFamily.Repo.Core.AWS
{
    public class AWSClientFactory : IAWSClientFactory
    {
        private AmazonDynamoDBClient _client = new AmazonDynamoDBClient(RegionEndpoint.APSoutheast2);

        private IAmazonDynamoDB GetDynamoDbClient()
        {
            return new AmazonDynamoDBClient(new BasicAWSCredentials("AKIAJ3GK4E3FECZ6CQJQ", "LEHCAi/ln5TfkpsJfG0i43E8LmeSj30xM96YlnkN"), RegionEndpoint.APSoutheast2);


            //    new BasicAWSCredentials("AKIAJ3GK4E3FECZ6CQJQ", "LEHCAi/ln5TfkpsJfG0i43E8LmeSj30xM96YlnkN"), 
            //RegionEndpoint.APSoutheast2));

        }

        public IDynamoDBContext GetDynamoDBContext()
        {
            return new DynamoDBContext(GetDynamoDbClient());
        }

        public IAmazonS3 GetS3Context()
        {
            return new AmazonS3Client(RegionEndpoint.APSoutheast2);
        }

    }
}

