﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.SimpleEmail;

namespace BryceFamily.Repo.Core.AWS
{
    public interface IAWSClientFactory
    {
        IDynamoDBContext GetDynamoDBContext();
        //IAmazonDynamoDB GetDynamoDbClient();
        IAmazonS3 GetS3Context();

        IAmazonSimpleEmailService GetSesClient();
    }
}
