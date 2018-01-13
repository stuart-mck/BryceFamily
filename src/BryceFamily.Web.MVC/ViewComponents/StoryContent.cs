using BryceFamily.Repo.Core.Read.Story;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.ViewComponents
{
    
    //public class StoryContent : ViewComponent
    //{
    //    private readonly IStoryReadRepository _storyReadRepository;

    //    public StoryContent(IStoryReadRepository storyReadRepository)
    //    {
    //        _storyReadRepository = storyReadRepository;
    //    }


    //    public async Task<IViewComponentResult> InvokeAsync(Guid storyId)
    //    {
    //        var story = await _storyReadRepository.Load(storyId, CancellationToken.None);
    //        return View(story);
    //    }

    //}
}
