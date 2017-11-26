using System.ComponentModel.DataAnnotations;

namespace BryceFamily.Web.MVC.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
