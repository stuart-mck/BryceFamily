using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("person")]
    public class Person : Entity
    {
        public Person()
        {
            Relationships = new List<SpousalRelationship>();
            Descendants = new List<Descendant>();
        }

        public int PersonID { get; set; }

        public string Clan { get; set; }

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

        public List<SpousalRelationship> Relationships { get; set; }

        public List<Descendant> Descendants { get; set; }

        public string PostCode { get; set; }

        public string EmailAddress { get; set; }

        public string SortKey { get; set; }
        public bool SubscribeToEmail { get; set; }

        public string Occupation { get; set; }
    }
}
