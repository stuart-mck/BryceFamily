using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<Spouse> Unions { get; set; }
        public Person Mother { get; set; }
        public Person Father { get; set; }


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
                Clan = Clan,
                ID = Id
            };
        }

        public static Person Map(Repo.Core.Model.Person person, List<Repo.Core.Model.Person> people)
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
                Clan = person.Clan,
                Id = person.ID,
                Mother = Map(people.FirstOrDefault(m => m.PersonID == person.MotherID), people),
                Father = Map(people.FirstOrDefault(m => m.PersonID == person.FatherID), people),
                Unions = MapUnions(person, people)
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
                Clan = person.Clan,
                Id = person.ID
            };
        }

        private static List<Spouse> MapUnions(Repo.Core.Model.Person person, List<Repo.Core.Model.Person> people)
        {
            var spouses = new List<Spouse>();

            foreach(var u in person.Relationships)
            {
                var spouse = new Spouse
                {
                    Partner = Map(people.FirstOrDefault(p => p.PersonID == u.PartnerID)),
                    Descendents = ResolveDependents(person.PersonID, u.PartnerID, people)
                };
            }

            return spouses;
        }

        private static List<Person> ResolveDependents(int firstParentId, int secondParentId, List<Repo.Core.Model.Person> people)
        {
            return people.Where(p => (p.FatherID == firstParentId && p.MotherID == secondParentId) ||
                                      (p.FatherID == secondParentId && p.MotherID == firstParentId))
                                      .Select(des => Map(des, people)).ToList();
        }
    }
}
