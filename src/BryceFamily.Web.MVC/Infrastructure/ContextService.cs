using BryceFamily.Repo.Core.Model;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class ContextService
    {
        private readonly HttpContext _context;
        private readonly ClanAndPeopleService _clanAndPeopleService;

        public ContextService(HttpContext  context, ClanAndPeopleService clanAndPeopleService)
        {
            _context = context;
            _clanAndPeopleService = clanAndPeopleService;
        }

        public Models.Person LoggedInPerson { get {
                if (_context.User != null)
                {
                    var userEmail = _context.User.Claims;
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
