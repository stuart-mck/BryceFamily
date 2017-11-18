using System;
using System.Collections.Generic;

namespace BryceFamily.Web.MVC.Models
{
    public class Union
    {
        public Person Partner1 { get; set; }
        public Person Partner2 { get; set; }
        public DateTime? DateOfUnion { get; set; }
        public DateTime? DateOfDissolution{ get; set; }
        public List<Person> Descendents { get; set; }
    }
}
