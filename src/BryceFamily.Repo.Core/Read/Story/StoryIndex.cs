using System;


namespace BryceFamily.Repo.Core.Read.Story
{
    public class StoryIndex
    {
        public Guid ID { get; set; }
        public Guid? PersonID { get; set; }

        public Guid AuthorID { get; set; }

        public string Title { get; set; }
    }
}
