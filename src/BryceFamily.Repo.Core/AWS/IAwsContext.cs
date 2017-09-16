using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace BryceFamily.Repo.Core.AWS
{
    public interface IAwsContext
    {
        IDynamoDbContext GetDynamoDbContext();
    }
}
