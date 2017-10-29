namespace BryceFamily.Repo.Core.Write.Query
{
    public class PersonIdentifier : IQueryParameter
    {
        public PersonIdentifier()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MiddleName { get; set; }
    }
}