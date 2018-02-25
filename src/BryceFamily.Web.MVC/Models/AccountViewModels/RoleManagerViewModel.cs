namespace BryceFamily.Web.MVC.Models.AccountViewModels
{
    public class RoleManagerViewModel
    {
        public string Email { get; set; }
        public bool IsInUserRole { get; set; }

        public bool IsInEditorRole { get; set; }

        public bool IsInSuperUserRole { get; set; }

        public string Id { get; set; }

        public bool Altered { get; set; }
    }
}
