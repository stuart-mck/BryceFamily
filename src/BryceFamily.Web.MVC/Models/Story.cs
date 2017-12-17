using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Read.Story;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BryceFamily.Web.MVC.Models
{
    public class Story
    {
        public Guid ID { get; set; }
        public string StoryTitle { get; set; }
        public int? StoryObject { get; set; }
        public string StoryContent { get; set; }


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
    }
}
