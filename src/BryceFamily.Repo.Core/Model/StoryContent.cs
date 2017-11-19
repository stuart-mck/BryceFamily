using Amazon.DynamoDBv2.DataModel;
using System;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("story")]
    public class StoryContent : Entity
    {
        public Guid? PersonID { get; set; }

        public Guid AuthorID { get; set; }

        public string StoryContents { get; set; }

        public string Title { get; set; }

    }
}
