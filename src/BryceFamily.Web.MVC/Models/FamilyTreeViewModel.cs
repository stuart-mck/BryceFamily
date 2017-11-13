using System;
using System.Collections.Generic;

namespace BryceFamily.Web.MVC.Models
{
    public class FamilyTreeViewModel
    {
        public string FirstNames { get; set; }

        public string LastName { get; set; }

        public int PersonId { get; set; }

        public int Level { get; set; }
        
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfDeath { get; set; }

        public List<FamilyTreeViewModel> Spouses { get; set; }

        public List<FamilyTreeViewModel> Descendents { get; set; }
    }
}
