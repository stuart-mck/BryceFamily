using BryceFamily.Repo.Core.AWS;
using System;

namespace BryceFamily.Repo.Core.Read.Users
{
    public class UserReadRepository
    {
        private readonly IAWSClientFactory _awsClientFactory;
        
        public UserReadRepository(IAWSClientFactory aWSClientFactory)
        {
            _awsClientFactory = aWSClientFactory;
        }

        public User GetUser(Guid userId)
        {
            return null;
        }
    }
}
