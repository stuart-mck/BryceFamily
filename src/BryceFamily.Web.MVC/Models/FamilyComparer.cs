using System.Collections.Generic;

namespace BryceFamily.Web.MVC.Models
{
    public class FamilyComparer : IComparer<FamilyClan>
    {
        public int Compare(FamilyClan x, FamilyClan y)
        {
            return (x.Family + x.FamilyName).CompareTo(y.Family + y.FamilyName);
        }
    }
}
