using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models
{
    public class NewChildModel
    {
        public NewChildModel()
        {

        }

        public NewChildModel(Person parent1, Person parent2)
        {
            Parent1 = parent1.Id;
            Parent2 = parent2.Id;
            Parents = $"{parent1?.FullName ?? "Not Registered"} and {parent2?.FullName ?? "Not Registered"}";
        }

        public int Parent1 { get; set; }

        public int Parent2 { get; set; }

        public string Parents { get; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Gender { get; set; }

        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
    }
}
