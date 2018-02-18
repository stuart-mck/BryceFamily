using BryceFamily.Repo.Core.Model;
using System;
using BryceFamily.Repo.Core.Write.Query;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using BryceFamily.Repo.Core.AWS;
using Amazon.DynamoDBv2.DocumentModel;
using System.Linq;

namespace BryceFamily.Repo.Core.Write.People
{
    public class UnionWriteRepository<TEntity, TId> : IWriteRepository<TEntity, TId>
        where TEntity : Entity<TId>
    {
        private readonly IAWSClientFactory _clientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;
        public UnionWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
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
            var personQuery = (UnionQuery)query;
            var dynamoContext = _clientFactory.GetDynamoDBContext();


            var dynamoOperationContext = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And,
                TableNamePrefix = "familybryce.",
                IndexName = "Members-index",
                
            };

            var queryResult = await dynamoContext.QueryAsync<TEntity>(BuildIndex(personQuery.Partner1Id, personQuery.Partner2Id), dynamoOperationContext).GetRemainingAsync(cancellationToken);
            return queryResult.FirstOrDefault();
        }

        public async Task Save(TEntity entity, CancellationToken cancellationToken)
        {
            var dynamoContext = _clientFactory.GetDynamoDBContext();
            var union = entity as Union;
            if (union.ID == Guid.Empty)
                union.ID = Guid.NewGuid();
            union.LastUpdated = DateTime.Now;
            union.Members = BuildIndex(union?.PartnerID, union?.Partner2ID);
            await dynamoContext.SaveAsync(union, _dynamoDBOperationConfig, cancellationToken);

        }

        private string BuildIndex(int? partner1Id, int? partner2Id)
        {
            return $"{partner1Id}_{partner2Id}";
        }
    }
}
