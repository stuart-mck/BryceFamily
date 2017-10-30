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
    public class PeopleReadRepository : IPersonReadRepository
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

        public async Task<List<Person>> SearchByName(string firstName, string lastName, string emailAddress, string occupation, CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();

            var scanConditions = new List<ScanCondition>();

            if (!string.IsNullOrEmpty(firstName))
                scanConditions.Add(new ScanCondition("FirstName", ScanOperator.Equal, firstName));

            if (!string.IsNullOrEmpty(lastName))
                scanConditions.Add(new ScanCondition("LastName", ScanOperator.Equal, lastName));

            if (!string.IsNullOrEmpty(emailAddress))
                scanConditions.Add(new ScanCondition("EmailAddress", ScanOperator.Equal, emailAddress));

            if (!string.IsNullOrEmpty(occupation))
                scanConditions.Add(new ScanCondition("Occupation", ScanOperator.Equal, occupation));


            return await dbContext.ScanAsync<Person>(scanConditions, _operationConfig).GetRemainingAsync(cancellationToken);

        }

    }
}
