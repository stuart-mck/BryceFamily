using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using BryceFamily.Repo.Core.AWS;
using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.FamilyEvents
{
    public class FamilyEventReadRepository : IFamilyEventReadRepository
    {
        private readonly IAWSClientFactory _awsClientFactory;
        private readonly DynamoDBOperationConfig _operationConfig;

        public FamilyEventReadRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig operationConfig)
        {
            _awsClientFactory = awsClientFactory;
            _operationConfig = operationConfig;
        }

        public async Task<List<FamilyEvent>> GetAllEventsStartingAfter(DateTime referenceDate, CancellationToken cancellation)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();

            var batchGetBatches = new List<BatchGet<FamilyEvent>>();

            return await dbContext.ScanAsync<FamilyEvent>(new List<ScanCondition>()
            {
                new ScanCondition("StartDate", ScanOperator.GreaterThan, referenceDate)
            }, _operationConfig).GetRemainingAsync(cancellation);
        }

        public async Task<List<FamilyEvent>> GetAllEvents(CancellationToken cancellation)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();

            var batchGetBatches = new List<BatchGet<FamilyEvent>>();

            return await dbContext.ScanAsync<FamilyEvent>(new List<ScanCondition>(), _operationConfig).GetRemainingAsync(cancellation);
        }

        public async Task<FamilyEvent> Load(Guid id, CancellationToken cancellationToken)
        {
            var dbContext = _awsClientFactory.GetDynamoDBContext();
            return await dbContext.LoadAsync<FamilyEvent>(id, cancellationToken);
        }
    }
}
