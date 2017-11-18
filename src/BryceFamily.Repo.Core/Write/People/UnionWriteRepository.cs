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
    public class UnionWriteRepository<TEntity, TId> : IWriteRepository<Union, Guid>
    {
        private readonly IAWSClientFactory _clientFactory;
        private readonly DynamoDBOperationConfig _dynamoDBOperationConfig;
        public UnionWriteRepository(IAWSClientFactory awsClientFactory, DynamoDBOperationConfig dynamoDBOperationConfig)
        {
            _clientFactory = awsClientFactory;
            _dynamoDBOperationConfig = dynamoDBOperationConfig;
        }

        public void Delete(Guid entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<Union> FindByQuery(IQueryParameter query, CancellationToken cancellationToken)
        {
            var personQuery = (UnionQuery)query;
            var dynamoContext = _clientFactory.GetDynamoDBContext();


            var dynamoOperationContext = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And,
                TableNamePrefix = "familybryce.",
                IndexName = "Members-index",
                
            };

            var queryResult = await dynamoContext.QueryAsync<Union>(BuildIndex(personQuery.Partner1Id, personQuery.Partner2Id), dynamoOperationContext).GetRemainingAsync(cancellationToken);
            return queryResult.FirstOrDefault();
            
        }

        public async Task Save(Union entity, CancellationToken cancellationToken)
        {
            var dynamoContext = _clientFactory.GetDynamoDBContext();
            if (entity.ID == Guid.Empty)
                entity.ID = Guid.NewGuid();
            entity.Members = BuildIndex(entity.PartnerID, entity.Partner2ID);
            await dynamoContext.SaveAsync(entity, _dynamoDBOperationConfig, cancellationToken);

        }

        private string BuildIndex(int partner1Id, int partner2Id)
        {
            return $"{partner1Id}_{partner2Id}";
        }
    }
}
