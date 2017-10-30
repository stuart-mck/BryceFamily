using System;

namespace BryceFamily.Web.MVC.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Clan { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobilePhone { get; set; }

        public string HomePhone { get; set; }

        public string Address { get; set; }
        public string Address1 { get; set; }

        public string Suburb { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public string EmailAddress { get; set; }

        public string Occupation { get; set; }

        public bool SubscribeToEmail { get; set; }

        public Repo.Core.Model.Person MapToEntity()
        {
            return new Repo.Core.Model.Person()
            {
                Address = Address,
                Address1 = Address1,
                Country = Country,
                EmailAddress = EmailAddress,
                FirstName = FirstName,
                HomePhone = HomePhone,
                LastName = LastName,
                MobilePhone = MobilePhone,
                PostCode = PostCode,
                State = State,
                SubscribeToEmail = SubscribeToEmail,
                Suburb = Suburb,
                Occupation = Occupation,
                Clan = Clan,
                ID = Id
            };
        }

        public static Person Map(Repo.Core.Model.Person person)
        {
            return new Person()
            {
                Address = person.Address,
                Address1 = person.Address1,
                Country = person.Country,
                EmailAddress = person.EmailAddress,
                FirstName = person.FirstName,
                HomePhone = person.HomePhone,
                LastName = person.LastName,
                MobilePhone = person.MobilePhone,
                PostCode = person.PostCode,
                State = person.State,
                SubscribeToEmail = person.SubscribeToEmail,
                Suburb = person.Suburb,
                Occupation = person.Occupation,
                Clan = person.Clan,
                Id = person.ID
            };
        }
    }
}
