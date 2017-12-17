using Amazon.DynamoDBv2.DataModel;
using System;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("story")]
    public class StoryContent : Entity<Guid>
    {
        public int? PersonID { get; set; }

        public int AuthorID { get; set; }

        public string StoryContents { get; set; }

        public string Title { get; set; }

    }
}
