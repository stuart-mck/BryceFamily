using System;

namespace BryceFamily.Web.MVC.Models
{
    public class Story
    {
        public string StoryTitle { get; set; }
        public Guid StoryObject { get; set; }
        public string StoryContent { get; set; }
    }
}
