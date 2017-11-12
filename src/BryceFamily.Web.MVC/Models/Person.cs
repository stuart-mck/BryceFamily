﻿using System;

namespace BryceFamily.Web.MVC.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Clan { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string Phone { get; set; }

        public string Address { get; set; }
        public string Address1 { get; set; }

        public string Suburb { get; set; }

        public string State { get; set; }

        public string PostCode { get; set; }

        public string EmailAddress { get; set; }

        public string Occupation { get; set; }

        public bool SubscribeToEmail { get; set; }
        public string MiddleName { get; private set; }

        public Repo.Core.Model.Person MapToEntity()
        {
            return new Repo.Core.Model.Person()
            {
                Address1 = Address,
                Address2 = Address1,
                EmailAddress = EmailAddress,
                FirstName = FirstName,
                Phone = Phone,
                LastName = LastName,
                PostCode = PostCode,
                State = State,
                SubscribeToEmail = SubscribeToEmail,
                Suburb = Suburb,
                Occupation = Occupation,
                Clan = int.Parse(Clan),
                ID = Id
            };
        }

        public static Person Map(Repo.Core.Model.Person person)
        {
            return new Person()
            {
                Address = person.Address1,
                Address1 = person.Address2,
                EmailAddress = person.EmailAddress,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                Phone = person.Phone,
                LastName = person.LastName,
                PostCode = person.PostCode,
                State = person.State,
                SubscribeToEmail = person.SubscribeToEmail,
                Suburb = person.Suburb,
                Occupation = person.Occupation,
                Clan = person.Clan.ToString(),
                Id = person.ID
            };
        }
    }
}
