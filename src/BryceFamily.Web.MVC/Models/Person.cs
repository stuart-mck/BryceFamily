using BryceFamily.Web.MVC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BryceFamily.Web.MVC.Models
{
    public class Person
    {
        public Person()
        {
            Unions = new List<Union>();
            Stories = new List<Story>();
            Galleries = new List<Gallery>();
        }

        public int Id { get; set; }
        
        public int Clan { get; set; }

        public string ClanName { get; set; }
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
        public List<Union> Unions { get; set; }
        public Person Mother { get; set; }
        public Person Father { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }
        public string Gender { get; private set; }
        public bool IsSpouse { get; internal set; }
        public Guid ParentRelationship { get; private set; }

        public List<Story> Stories { get; set; }

        public List<Gallery> Galleries { get; set; }

        public bool IsClanManager { get; set; }

        public Repo.Core.Model.Person MapToEntity(ClanAndPeopleService clanAndPeopleService)
        {
            return new Repo.Core.Model.Person()
            {
                Address1 = Address,
                Address2 = Address1,
                EmailAddress = EmailAddress,
                FirstName = FirstName,
                MiddleName = MiddleName,
                Phone = Phone,
                LastName = LastName,
                PostCode = PostCode,
                State = State,
                SubscribeToEmail = SubscribeToEmail,
                Suburb = Suburb,
                Occupation = Occupation,
                Clan = clanAndPeopleService.Clans.First(t => t.Id == Clan).FormattedName,
                DateOfBirth = DateOfBirth,
                DateOfDeath = DateOfDeath,
                IsSpouse = IsSpouse,
                Gender = Gender,
                ID = Id
            };
        }

  

        public static Person FlatMap(Repo.Core.Model.Person person, ClanAndPeopleService clanAndPeopleService)
        {
            var clan = clanAndPeopleService.Clans.FirstOrDefault(t => t.FormattedName == person.Clan);

            if (person == null)
                return null;
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
                ClanName = clan == null ? string.Empty : clan.FormattedName,
                Clan = clan == null ? 0 : clan.Id,
                Id = person.ID,
                IsSpouse = person.IsSpouse,
                DateOfBirth = person.DateOfBirth,
                DateOfDeath = person.DateOfDeath,
                Gender = person.Gender,
                //Mother = Map(peopleLookup.FirstOrDefault(m => person.MotherID == m.PersonID), peopleLookup),
                //Father = peopleLookup.FirstOrDefault(m => person.FatherID == m.PersonId),
                ParentRelationship = person.ParentRelationship,
                IsClanManager  = person.IsClanManager
            };
        }

    }
}
