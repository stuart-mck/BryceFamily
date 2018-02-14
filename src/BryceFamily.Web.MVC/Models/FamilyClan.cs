namespace BryceFamily.Web.MVC.Models
{
    public class FamilyClan
    {
        public FamilyClan(int id, string family, string familyName)
        {
            Id = id;
            Family = family;
            FamilyName = familyName;
        }

        public int Id { get; }
        public string Family { get; }
        public string FamilyName { get; }

        public string FormattedName => $"{Family}, {FamilyName}";
    }
}
