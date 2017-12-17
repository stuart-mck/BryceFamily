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
    public class PeopleWriteRepository<TEntity, TId> : IWriteRepository<TEntity, TId> 
        where TEntity : Entity<TId>
    {
        private readonly IAWSClientFactory _clientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;
        public PeopleWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _clientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }

        public void Delete(TId entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> FindByQuery(IQueryParameter query, CancellationToken cancellationToken)
        {
            var personQuery = (PersonIdentifier)query;
            var dynamoContext = _clientFactory.GetDynamoDBContext();


            var dynamoOperationContext = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And,
                TableNamePrefix = "familybryce."
            };

            
            if (personQuery.PersonId.HasValue)
            {
                return await dynamoContext.LoadAsync<TEntity>(personQuery.PersonId.Value, _dynamoDBOperationConfig, cancellationToken);
            }
            else
            {
                dynamoOperationContext.QueryFilter.Add(new ScanCondition("FirstName", ScanOperator.Equal, personQuery.FirstName));
                dynamoOperationContext.QueryFilter.Add(new ScanCondition("LastName", ScanOperator.Equal, personQuery.LastName));
                dynamoOperationContext.QueryFilter.Add(new ScanCondition("EmailAddress", ScanOperator.Equal, personQuery.EmailAddress));
                var person = await dynamoContext.QueryAsync<TEntity>(dynamoOperationContext).GetRemainingAsync();
                return person.FirstOrDefault();
            }
            
        }

        public async Task Save(TEntity entity, CancellationToken cancellationToken)
        {
            var dynamoContext = _clientFactory.GetDynamoDBContext();
            var person = entity as Person;
            //if (entity.ID == Guid.Empty)
            //    entity.ID = Guid.NewGuid();
            person.ParentKey = $"{person.FatherID}_{person.MotherID}";
            await dynamoContext.SaveAsync(person, _dynamoDBOperationConfig, cancellationToken);
        }


    }
}
