using DevOne.Security.Cryptography.BCrypt;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt(_rounds));
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (BCryptHelper.CheckPassword(providedPassword, hashedPassword))
                return PasswordVerificationResult.Success;
            return PasswordVerificationResult.Failed;

        }
    }
}
