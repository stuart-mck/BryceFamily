using Amazon.DynamoDBv2.DataModel;
using System;


namespace BryceFamily.Repo.Core.Read.Story
{
    [DynamoDBTable("story")]
    public class StoryIndex
    {
        public Guid ID { get; set; }
        public int? PersonID { get; set; }

        public int AuthorID { get; set; }

        public string Title { get; set; }
    }
}
