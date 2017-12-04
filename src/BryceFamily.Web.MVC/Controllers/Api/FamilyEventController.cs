using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using BryceFamily.Web.MVC.Infrastructure.Authentication;
using BryceFamily.Repo.Core.Read.FamilyEvents;
using System.Threading;

namespace BryceFamily.Web.MVC.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/FamilyEvent")]
    public class FamilyEventController : Controller
    {
        private readonly IFamilyEventReadRepository _familyEventReadModel;

        public FamilyEventController(IFamilyEventReadRepository familyEventReadModel)
        {
            _familyEventReadModel = familyEventReadModel;
        }

        [HttpGet]
        [Authorize(Roles = RoleNameConstants.AllAdminRoles)]
        public async Task<IEnumerable<FamilyEvent>> Get()
        {
            return (await _familyEventReadModel.GetAllEvents(CancellationToken.None)).Select(FamilyEvent.Map);
        }
    }
}