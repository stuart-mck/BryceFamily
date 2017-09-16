using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace BryceFamily.Repo.Core.AWS
{
    public interface IAWSClientFactory
    {
        IDynamoDBContext GetDynamoDBContext();
    }
}
