using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Read.People.DTO
{
    public class PeopleSearchQueryResult
    {
        public Guid ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Occupation { get; set; }

        public string  EmailAddress { get; set; }
    }
}
