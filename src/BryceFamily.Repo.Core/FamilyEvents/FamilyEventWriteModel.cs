using BryceFamily.Repo.Core.AWS;
using BryceFamily.Repo.Core.Model;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.FamilyEvents
{
    public class FamilyEventWriteModel
    {
        private readonly IAWSClientFactory _awsClientFactory;

        public FamilyEventWriteModel(IAWSClientFactory awsClientFactory)
        {
            _awsClientFactory = awsClientFactory;
        }

        public async Task Save(FamilyEvent familyEvent)
        {
            var context = _awsClientFactory.GetDynamoDBContext();
            await context.SaveAsync(familyEvent);
        }
      
    }
}
