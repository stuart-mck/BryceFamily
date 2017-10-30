using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
