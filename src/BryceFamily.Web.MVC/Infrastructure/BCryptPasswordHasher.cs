using Microsoft.AspNetCore.Identity;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {

        private const int _rounds = 12;
        public BCryptPasswordHasher()
        {
            
        }
        
        public string HashPassword(TUser user, string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password,_rounds);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword) ? PasswordVerificationResult.Success 
                : PasswordVerificationResult.Failed;

        }
    }
}
