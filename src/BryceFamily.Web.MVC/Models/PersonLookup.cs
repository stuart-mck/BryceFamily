using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace BryceFamily.Web.MVC.Models
{
    public class PersonLookup
    {

        public Guid ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Clan { get; set; }

        public int? MotherId { get; set; }

        public Person Mother { get; }

        public Person Father { get; }

        public int? FatherId { get; set; }

        public int PersonId { get; set; }

        public List<Union> Spouses { get; set; }
    }
}
