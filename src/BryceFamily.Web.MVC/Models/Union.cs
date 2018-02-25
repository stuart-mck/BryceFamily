using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BryceFamily.Web.MVC.Models
{
    public class Union
    {
        public Guid Id { get; set; }
        public Person Partner1 { get; set; }
        public Person Partner2 { get; set; }
        [DisplayName("Date of Union")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateOfUnion { get; set; }
        [DisplayName("Date of Dissolution")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateOfDissolution{ get; set; }
        public List<Person> Descendents { get; set; }
    }
}
