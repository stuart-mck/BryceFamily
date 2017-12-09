using Microsoft.AspNetCore.Http;
using System.Linq;
namespace BryceFamily.Web.MVC.Infrastructure
{
    public class ContextService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ClanAndPeopleService _clanAndPeopleService;

        public ContextService(IHttpContextAccessor  context, ClanAndPeopleService clanAndPeopleService)
        {
            _context = context;
            _clanAndPeopleService = clanAndPeopleService;
        }

        public Models.Person LoggedInPerson { get {
                if (_context.HttpContext.User != null)
                {
                    var userEmail = _context.HttpContext.User.Identity.Name;
                    return _clanAndPeopleService.People.FirstOrDefault(p => p.EmailAddress == userEmail);
                }
                return null;
            } }

        public bool IsLoggedIn
        {
            get
            {
                return LoggedInPerson != null;
            }
        }

        public bool EditMode { get; set; }
        

        public bool IsClanManager { get; set; }

        public bool IsSuperUser { get; set; }

    }
}
