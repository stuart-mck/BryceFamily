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

    public class EventComparer : IComparer<FamilyEvent>
    {
        public int Compare(FamilyEvent x, FamilyEvent y)
        {
            return x.StartDate.CompareTo(y.StartDate);
        }
    }
}
