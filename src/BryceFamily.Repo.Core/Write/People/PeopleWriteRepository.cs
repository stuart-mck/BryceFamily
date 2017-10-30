using System;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.AWS;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Linq;
using BryceFamily.Repo.Core.Write.Query;
using System.Threading;

namespace BryceFamily.Repo.Core.Write.People
{
    public class PeopleWriteRepository<TEntity, TId> : IWriteRepository<Person, Guid> 
    {
        private readonly IAWSClientFactory _clientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;
        public PeopleWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _clientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<Person> FindByQuery(IQueryParameter query)
        {
            var personQuery = (PersonIdentifier)query;
            var dynamoContext = _clientFactory.GetDynamoDBContext();

            var dynamoOperationContext = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And,
                TableNamePrefix = "familybryce."
            };

            dynamoOperationContext.QueryFilter.Add(new ScanCondition("FirstName", ScanOperator.Equal, personQuery.FirstName));
            dynamoOperationContext.QueryFilter.Add(new ScanCondition("LastName", ScanOperator.Equal, personQuery.LastName));
            dynamoOperationContext.QueryFilter.Add(new ScanCondition("EmailAddress", ScanOperator.Equal, personQuery.EmailAddress));

            var person = await dynamoContext.QueryAsync<Person>(dynamoOperationContext).GetRemainingAsync();
            return person.FirstOrDefault();
        }

        public async Task Save(Person entity, CancellationToken cancellationToken)
        {
            var dynamoContext = _clientFactory.GetDynamoDBContext();
            entity.ID = Guid.NewGuid();
            entity.SortKey = $"{entity.FirstName}_{entity.LastName}_{entity.EmailAddress}";
            await dynamoContext.SaveAsync(entity, _dynamoDBOperationConfig, cancellationToken);
        }
    }
}
