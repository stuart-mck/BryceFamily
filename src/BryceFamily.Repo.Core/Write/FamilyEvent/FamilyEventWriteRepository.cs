using Amazon.DynamoDBv2.DataModel;
using BryceFamily.Repo.Core.AWS;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Write;
using System;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Write.Query;
using System.Threading;

namespace BryceFamily.Repo.Core.FamilyEvents
{
    public class FamilyEventWriteRepository<TEntity, TId> : IWriteRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        private readonly IAWSClientFactory _awsClientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;

        public FamilyEventWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _awsClientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }

        public void Delete(TId entityId)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindByQuery(IQueryParameter repository, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

       
        public async Task Save(TEntity entity, CancellationToken cancellationToken)
        {
            var context = _awsClientFactory.GetDynamoDBContext();

            await context.SaveAsync(entity, _dynamoDBOperationConfig , cancellationToken);
        }
    }
}
