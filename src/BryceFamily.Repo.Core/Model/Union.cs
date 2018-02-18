using Amazon.DynamoDBv2.DataModel;
using System;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("union")]
    public class Union : Entity<Guid>
    {
        public int? PartnerID { get; set; }

        public int? Partner2ID { get; set; }

        //public string Members { get; set; }

        public DateTime? MarriageDate { get; set; }

        public DateTime? DivorceDate { get; set; }

        public bool IsEnded { get; set; }

        public string Members { get; set; }

    }
}
