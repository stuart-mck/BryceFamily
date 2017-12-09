namespace BryceFamily.Web.MVC.Infrastructure
{
    public class ContextService
    {
        public Models.Person LoggedInPerson { get; set; }

        public bool IsLoggedIn
        {
            get
            {
                return LoggedInPerson != null;
            }
        }

        public bool EditMode { get; set; }
        

        public bool IsClanManager { get; set; }

        public bool IsSuperUser { get; set; }

    }
}
