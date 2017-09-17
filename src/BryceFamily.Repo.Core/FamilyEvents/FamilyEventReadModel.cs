using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using BryceFamily.Repo.Core.AWS;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace BryceFamily.Repo.Core.FamilyEvents
{
    public class FamilyEventReadModel<TEntity, TId> : IReadModel<FamilyEvent, Guid>
    {
        private readonly IAWSClientFactory _awsContext;
        private const string _TABLENAME = "familybryce.familyevent";

        public FamilyEventReadModel(IAWSClientFactory awsContext)
        {
            _awsContext = awsContext;
        }

        public async Task<FamilyEvent> Load(Guid id, CancellationToken cancellationToken)
        {
            var context = _awsContext.GetDynamoDBContext();
            return await context.LoadAsync<FamilyEvent>(id);
        }

        public async Task<List<FamilyEvent>> GetByQuery(DateTime startDate, DateTime endDate)
        {
            var context = _awsContext.GetDynamoDBContext();
            var startDateString = startDate.ToString(AWSSDKUtils.ISO8601DateFormat);
            var endDateString = endDate.ToString(AWSSDKUtils.ISO8601DateFormat);

            var queryRequest = new QueryRequest()
            {
                TableName = _TABLENAME,
                IndexName = "familyEventByDate",
                KeyConditionExpression = "startDate > :v_startDate and startDate < :v_endDate",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":v_startDate", new AttributeValue { S =  startDateString }},
                    {":v_endDate", new AttributeValue { S =  endDateString }}
                },
            };

            return await context.QueryAsync<FamilyEvent>(queryRequest).GetRemainingAsync();
            
        }

        public Task<IQueryable<FamilyEvent>> AsQueryable()
        {
            throw new NotImplementedException();
        }
    }
}
