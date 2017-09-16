using System.Collections.Generic;

namespace BryceFamily.Repo.Core.Model
{
    public class Person : Entity
    {

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


        public ICollection<Image> Images { get; set; }

    }
}
