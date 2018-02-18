namespace BryceFamily.Web.MVC.Models
{
    public class FamilyClan
    {
        public FamilyClan(int id, string family, string familyName, int order)
        {
            Id = id;
            Family = family;
            FamilyName = familyName;
        }

        public int Id { get; }
        public string Family { get; }
        public string FamilyName { get; }
        public int DisplayOrder { get; set; }
        public string FormattedName => $"{Family}, {FamilyName}";
    }
}
