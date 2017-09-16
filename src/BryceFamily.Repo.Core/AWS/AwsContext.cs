using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.AWS
{
    public class AwsContext
    {
        private AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public IAmazonDynamoDBClient GetDynamoDbClient()
        {
            return new AmazonDynamoDBClient();
        }
    }
}
