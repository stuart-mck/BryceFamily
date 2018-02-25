using BryceFamily.Repo.Core.Model;
using BryceFamily.Web.MVC.Infrastructure;
using System;
using System.Linq;

namespace BryceFamily.Web.MVC.Models.Stories
{
    public class Story
    {
        public Guid ID { get; set; }
        public string StoryTitle { get; set; }
        public int? StoryObject { get; set; }
        public string StoryContent { get; set; }
        public Person StoryPerson { get; set; }

        public static Story MapToIndex(StoryContent story)
        {
            return new Story
            {
                StoryObject = story.PersonID,
                StoryTitle = story.Title,
                ID = story.ID,
                StoryContent = story.StoryContents
            };
        }

        public static Story Map(StoryContent story, ClanAndPeopleService clanAndPeopleService)
        {
            return new Story
            {
                StoryObject = story.PersonID,
                StoryTitle = story.Title,
                ID = story.ID,
                StoryContent = story.StoryContents,
                StoryPerson = clanAndPeopleService.People.FirstOrDefault(t => t.Id == story.PersonID)
            };
        }

    }
}
