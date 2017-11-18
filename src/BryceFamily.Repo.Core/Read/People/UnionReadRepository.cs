using Amazon.DynamoDBv2.DataModel;
using BryceFamily.Repo.Core.AWS;
using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.People
{
    public class UnionReadRepository : IUnionReadRepository
    {
        private readonly IAWSClientFactory _awsClientFactory;
        private readonly DynamoDBOperationConfig _operationConfig;

        public UnionReadRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig operationConfig)
        {
            _awsClientFactory = awsClientFactory;
            _operationConfig = operationConfig;
        }

        public async Task<List<Union>> GetAllUnions(CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();
            var results = await dbContext.ScanAsync<Union>(new List<ScanCondition>(), _operationConfig).GetRemainingAsync(cancellationToken);
            return results;
        }

        public Task<Union> Load(Guid id, CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();
            return dbContext.LoadAsync<Union>(id, _operationConfig);
        }
    }
}
