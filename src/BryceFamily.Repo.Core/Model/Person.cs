using Amazon.DynamoDBv2.DataModel;
using System;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("person")]
    public class Person : Entity<int>
    {
        public Person()
        {
            
        }


        public string FirstName { get; set; }

        public string MiddleName{ get; set; }

        public string LastName { get; set; }

        public string MaidenName { get; set; }

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Suburb { get; set; }

        public string State { get; set; }

        public int? MotherID { get; set; }

        public int? FatherID { get; set; }

        public Guid ParentRelationship { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? YearOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public int? YearOfDeath { get; set; }

        public string PostCode { get; set; }

        public string EmailAddress { get; set; }

        public bool SubscribeToEmail { get; set; }

        public string Occupation { get; set; }

        public string Gender { get; set; }

        public string ParentKey { get; internal set; }

        public bool IsSpouse { get; set; }


        public bool IsClanManager { get; set; }
        public int? ClandId { get; set; }
    }
}
