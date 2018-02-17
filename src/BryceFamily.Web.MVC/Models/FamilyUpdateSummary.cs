using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models
{
    public class FamilyUpdateSummary
    {
        public string FamilyName { get; set; }

        public int ClanId { get; set; }

        public DateTime LastUpdate { get; set; }

    }
}
