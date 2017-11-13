using System;
using System.Collections.Generic;
using System.Text;

namespace BryceFamily.Repo.Core.Read.People.DTO
{
    public class LightWeightPerson
    {
        public Guid ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Clan { get; set; }

        public int PersonId { get; set; }

        public int? MotherId { get; set; }
        public int? FatherId { get; set; }


    }
}
