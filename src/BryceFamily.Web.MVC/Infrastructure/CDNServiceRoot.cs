namespace BryceFamily.Web.MVC.Infrastructure
{
    public class CDNServiceRoot
    {
        

        public CDNServiceRoot(string serviceRoot)
        {
            ServiceRoot = serviceRoot;
        }

        public string ServiceRoot { get; private set; }
    }
}
