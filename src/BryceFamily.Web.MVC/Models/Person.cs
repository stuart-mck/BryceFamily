using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Models
{
    public class Person
    {
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

        public string Email { get; set; }

        public bool SubscribeToSMS { get; set; }

        public bool SubscribeToEmail { get; set; }

        public Repo.Core.Model.Person MapToEntity()
        {
            return new Repo.Core.Model.Person()
            {
                Address = Address,
                Address1 = Address1,
                Country = Country,
                Email = Email,
                FirstName = FirstName,
                HomePhone = HomePhone,
                LastName = LastName,
                MobilePhone = MobilePhone,
                PostCode = PostCode,
                State = State,
                SubscribeToEmail = SubscribeToEmail,
                SubscribeToSMS = SubscribeToSMS,
                Suburb = Suburb
            };
        }

        public static Person Map(Repo.Core.Model.Person person)
        {
            return new Person()
            {
                Address = person.Address,
                Address1 = person.Address1,
                Country = person.Country,
                Email = person.Email,
                FirstName = person.FirstName,
                HomePhone = person.HomePhone,
                LastName = person.LastName,
                MobilePhone = person.MobilePhone,
                PostCode = person.PostCode,
                State = person.State,
                SubscribeToEmail = person.SubscribeToEmail,
                SubscribeToSMS = person.SubscribeToSMS,
                Suburb = person.Suburb
            };
        }
    }
}
