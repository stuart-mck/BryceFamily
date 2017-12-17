using Amazon.DynamoDBv2.DataModel;
using System;


namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("familyEvent")]
    public class FamilyEvent : Entity<Guid>
    {
        
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EventLocation Location { get; set; }

        public string Details { get; set; }

        public EventType EventType { get; set; }

        public EventStatus EventStatus { get; set; }

        public string OrganiserName { get; set; }

        public string OrganiserContact { get; set; }

        public string OrganiserEmail { get; set; }

    }

}
