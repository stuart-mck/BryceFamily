using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BryceFamily.Web.MVC.Models
{
    public class Union
    {
        public Guid Id { get; set; }
        public Person Partner1 { get; set; }
        public Person Partner2 { get; set; }
        [DisplayName("Date of Union")]
        public DateTime? DateOfUnion { get; set; }
        [DisplayName("Date of Dissolution")]
        public DateTime? DateOfDissolution{ get; set; }
        public List<Person> Descendents { get; set; }
    }
}
