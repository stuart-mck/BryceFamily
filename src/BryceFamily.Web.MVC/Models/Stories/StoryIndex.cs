using BryceFamily.Repo.Core.Model;
using BryceFamily.Web.MVC.Infrastructure;
using System;
using System.Linq;

namespace BryceFamily.Web.MVC.Models.Stories
{
    public class StoryIndex
    {
        public Guid ID { get; set; }
        public string StoryTitle { get; set; }
        public string StoryObject { get; set; }
        public Person StoryPerson { get; set; }


        public static StoryIndex MapToIndex(Repo.Core.Read.Story.StoryIndex story, ClanAndPeopleService clanAndPeopleService)
        {
            return new StoryIndex
            {
                StoryObject = story.PersonID.HasValue && story.PersonID > 0 ? 
                                clanAndPeopleService.People.FirstOrDefault(t => t.Id == story.PersonID).FullName :
                                string.Empty,
                StoryPerson = story.PersonID.HasValue && story.PersonID > 0 ?
                                clanAndPeopleService.People.FirstOrDefault(t => t.Id == story.PersonID):
                                null,
                StoryTitle = story.Title,
                ID = story.ID
            };
        }
    }
}
