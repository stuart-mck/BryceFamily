using System.ComponentModel.DataAnnotations;

namespace BryceFamily.Web.MVC.Models.AccountViewModels
{
    public class RegistrationRequest
    {
        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; }

        [Required, MaxLength(255), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, MaxLength(255), Display(Name = "Last Name")]
        public string LastName { get; set; }
        
    }
}
