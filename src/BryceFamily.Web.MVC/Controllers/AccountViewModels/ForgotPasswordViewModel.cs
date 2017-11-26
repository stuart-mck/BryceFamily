using System.ComponentModel.DataAnnotations;

namespace BryceFamily.Web.MVC.Controllers.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
