using Amazon.DynamoDBv2.DataModel;

namespace BryceFamily.Repo.Core.Model
{
    [DynamoDBTable("person")]
    public class Person : Entity
    {

        public Person():base()
        {

        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobilePhone { get; set; }

        public string HomePhone { get; set; }

        public string Address { get; set; }
        public string Address1 { get; set; }

        public string Suburb { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public string Email { get; set; }

        public bool SubscribeToEmail { get; set; }
        public string Occupation { get; set; }
    }
}
