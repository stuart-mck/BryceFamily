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

        public async Task<List<Person>> GetAllPeople(CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();
            var results = await dbContext.ScanAsync<Person>(new List<ScanCondition>(), _operationConfig).GetRemainingAsync(cancellationToken);
            return results;
        }

        public async Task<List<Person>> GetChildrenByParents(int? fatherId, int? motherId, CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();

            var dynamoOperationContext = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And,
                TableNamePrefix = "familybryce."
            };

            try
            {
                dynamoOperationContext.IndexName = "ParentKey-index";
                var queryResult = await dbContext.QueryAsync<Person>($"{fatherId}_{motherId}", dynamoOperationContext).GetRemainingAsync(cancellationToken);
                return queryResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<Person> Load(int id, CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();
            return dbContext.LoadAsync<Person>(id, _operationConfig);
        }

        //public async Task<Person> Load(int personId, CancellationToken cancellationToken)
        //{
        //    var dbContext = _awsClientFactory.GetDynamoDBContext();

        //    var dynamoOperationContext = new DynamoDBOperationConfig()
        //    {
        //        ConditionalOperator = ConditionalOperatorValues.And,
        //        TableNamePrefix = "familybryce."
        //    };

        //    dynamoOperationContext.IndexName = "PersonID-index";
        //    var queryResult = await dbContext.QueryAsync<Person>(personId, dynamoOperationContext).GetRemainingAsync(cancellationToken);
        //    return queryResult.FirstOrDefault();
        //}

        public async Task<List<Person>> SearchByName(string clan, string firstName, string lastName, string emailAddress, string occupation, CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();

            var scanConditions = new List<ScanCondition>();

            if (!string.IsNullOrEmpty(clan))
                scanConditions.Add(new ScanCondition("Clan", ScanOperator.Equal, clan));

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
