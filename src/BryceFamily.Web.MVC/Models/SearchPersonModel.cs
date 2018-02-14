using System.ComponentModel;

namespace BryceFamily.Web.MVC.Models
{
    public class SearchPersonModel
    {


        [DisplayName("Family")]
        public int Clan { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; } 

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Occupation")]
        public string Occupation { get; set; }

    }
}
