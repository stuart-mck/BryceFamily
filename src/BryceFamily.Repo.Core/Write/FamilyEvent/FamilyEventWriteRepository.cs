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
    public class FamilyEventWriteRepository<TEntity, TId> : IWriteRepository<FamilyEvent, Guid>
    {
        private readonly IAWSClientFactory _awsClientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;

        public FamilyEventWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _awsClientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public Task<FamilyEvent> FindByQuery(IQueryParameter repository)
        {
            throw new NotImplementedException();
        }

       

        public async Task Save(FamilyEvent familyEvent, CancellationToken cancellationToken)
        {
            var context = _awsClientFactory.GetDynamoDBContext();
            if (familyEvent.ID == Guid.Empty)
                familyEvent.ID = Guid.NewGuid();

            await context.SaveAsync(familyEvent, _dynamoDBOperationConfig , cancellationToken);
        }
    }
}
