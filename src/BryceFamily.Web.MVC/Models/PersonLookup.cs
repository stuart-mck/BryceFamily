using System;
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

        public int? FatherId { get; set; }

        public int PersonId { get; set; }
    }
}
