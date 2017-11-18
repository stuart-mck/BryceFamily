using System;
using System.Collections.Generic;

namespace BryceFamily.Web.MVC.Models
{
    public class Spouse
    {
        public Person Partner { get; set; }
        public DateTime? DateOfUnion { get; set; }
        public DateTime? DateOfDissolution{ get; set; }
        public List<Person> Descendents { get; set; }
    }
}
