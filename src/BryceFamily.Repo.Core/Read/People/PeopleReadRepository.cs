using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BryceFamily.Repo.Core.AWS;
using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.People
{
    public class PeopleReadRepository
    {
        private readonly IAWSClientFactory _awsClientFactory;
        private readonly DynamoDBOperationConfig _operationConfig;

        public PeopleReadRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig operationConfig)
        {
            _awsClientFactory = awsClientFactory;
            _operationConfig = operationConfig;
        }

        public Task<Person> Load(Guid id, CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();
            return dbContext.LoadAsync<Person>(id);
        }

        public async Task<List<Person>> SearchByName(string firstName, string lastName, string emailAddress, CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();
            var dynamoOperationContext = new DynamoDBOperationConfig()
            {
                IndexName = "person_by_name",
                ConditionalOperator = ConditionalOperatorValues.Or,
                TableNamePrefix = "familybryce."
            };

            dynamoOperationContext.QueryFilter.Add(new ScanCondition("FirstName", ScanOperator.Equal, firstName));
            dynamoOperationContext.QueryFilter.Add(new ScanCondition("LastName", ScanOperator.Equal, lastName));
            dynamoOperationContext.QueryFilter.Add(new ScanCondition("EmailAddress", ScanOperator.Equal, emailAddress));

            return await dbContext.QueryAsync<Person>(dynamoOperationContext).GetRemainingAsync(cancellationToken);

        }

    }
}
