using Microsoft.AspNetCore.Mvc;
using BryceFamily.Web.MVC.Models;
using BryceFamily.Web.MVC.Infrastructure;
using System.Linq;
using System;
using System.Threading.Tasks;
using BryceFamily.Repo.Core.Write;
using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Write.Query;
using System.Threading;
using BryceFamily.Repo.Core.Read.Story;
using Microsoft.AspNetCore.Authorization;
using BryceFamily.Web.MVC.Infrastructure.Authentication;

namespace BryceFamily.Web.MVC.Controllers
{
    public class HistoryController : BaseController
    {
        private readonly ClanAndPeopleService _clanService;
        private readonly IWriteRepository<StoryContent, Guid> _writeRepository;
        private readonly ContextService _contextService;
        private readonly IStoryReadRepository _storyReadRepository;

        public HistoryController(ClanAndPeopleService clanService, IWriteRepository<StoryContent, Guid>  writeRepository, ContextService contextService, IStoryReadRepository storyReadRepository):base("History", "history")
        {
            _clanService = clanService;
            _writeRepository = writeRepository;
            _contextService = contextService;
            _storyReadRepository = storyReadRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Story()
        {
            return View(new StoryWriteModel());
        }

        [HttpPost]
        [Authorize(Roles = RoleNameConstants.AdminRole)]
        public async Task<IActionResult> Story(StoryWriteModel storyWriteModel)
        {
            if (ModelState.IsValid)
            {
                var cancellationToken = CancellationToken.None;
                var existing = await _writeRepository.FindByQuery(new StoryQuery()
                {
                    StoryId = storyWriteModel.StoryID
                }, cancellationToken);

                if (existing == null)
                    existing = new StoryContent
                    {
                        AuthorID = _contextService.LoggedInPerson.Id,
                        PersonID = storyWriteModel.PersonID,
                        ID = Guid.NewGuid(),
                        StoryContents = storyWriteModel.Story,
                        Title = storyWriteModel.Title
                    };
                await _writeRepository.Save(existing, cancellationToken);
            }

            return View(storyWriteModel);
        }

        [Route("History/Tree/{id}")]
        public IActionResult Tree (int id)
        {
            Models.Person startNode;
            if (id < 1)
                startNode = _clanService.People.First(p => p.IsSpouse == false && p.Mother == null & p.Father == null);
            else
                startNode = _clanService.People.FirstOrDefault(p => p.Id == id);

            if (startNode == null)
                return BadRequest("Invalid Person reference");

            return View(startNode);
        }

        [Route("History/Stories/{id}")]
        public IActionResult StoriesForPerson(int id)
        {
            var person = _clanService.People.First(t => t.Id == id);
            

            return View(person.Stories);
        }
    }
}