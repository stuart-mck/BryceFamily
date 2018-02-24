using AspNetCore.Identity.DynamoDB;
using BryceFamily.Web.MVC.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class ContextService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ClanAndPeopleService _clanAndPeopleService;
        private readonly DynamoRoleUsersStore<DynamoIdentityRole, DynamoIdentityUser> _roleStore;
        private readonly UserManager<DynamoIdentityUser> _userStore;

        public ContextService(IHttpContextAccessor  context, ClanAndPeopleService clanAndPeopleService, DynamoRoleUsersStore<DynamoIdentityRole, DynamoIdentityUser> roleStore, UserManager<DynamoIdentityUser> userStore)
        {
            _context = context;
            _clanAndPeopleService = clanAndPeopleService;
            _roleStore = roleStore;
            _userStore = userStore;
        }

        public Models.Person LoggedInPerson
        {
            get
            {
                if (_context.HttpContext.User != null)
                {
                    var userEmail = _context.HttpContext.User.Identity.Name;
                    return userEmail == null ? null : _clanAndPeopleService.People.FirstOrDefault(p => p.EmailAddress == userEmail);
                }
                return null;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return _context.HttpContext.User.Claims.Any();
            }
        }

        

        public bool IsClanManager {
            get
            {
                var person = LoggedInPerson;
                return person != null && person.IsClanManager;
            }
        }

        public bool IsSuperUser()
        {
            
            var person = LoggedInPerson;
            if (person == null)
                return false;
            var user = _userStore.FindByEmailAsync(person.EmailAddress).Result;
            var roles = _roleStore.GetRolesAsync(user, CancellationToken.None).Result;
            if (roles.Any(t => t == RoleNameConstants.SuperAdminRole))
                return true;
            return false;
        }

        public bool IsLoggedInEditor()
        {
            var person = LoggedInPerson;
            if (person == null)
                return false;

            var user = _userStore.FindByEmailAsync(person.EmailAddress).Result;
            var roles = _roleStore.GetRolesAsync(user, CancellationToken.None).Result;
            if (roles.Any(t => t == RoleNameConstants.AllAdminRoles))
                return true;
            return false;
        }

    }
}
