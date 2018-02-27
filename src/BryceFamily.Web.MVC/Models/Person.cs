using BryceFamily.Web.MVC.Infrastructure;
using BryceFamily.Web.MVC.Models.Stories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Family")]
        public int? ClanId { get; set; }

        [DisplayName("Family")]
        public string ClanName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Middle Name")]
        public string MiddleName { get; private set; }
        [DisplayName("Maiden Name")]
        public string MaidenName { get; private set; }

        public string Phone { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string EmailAddress { get; set; }
        public string Occupation { get; set; }
        public bool SubscribeToEmail { get; set; }
        
        public List<Union> Unions { get; set; }
        public Person Mother { get; set; }
        public Person Father { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? DateOfBirth { get; set; }

        public int? YearOfBirth { get; set; }

        [DisplayName("Passed")]
        public DateTime? DateOfDeath { get; set; }

        public int Age {
            get
            {
                if (DateOfBirth.HasValue && DateOfDeath.HasValue)
                    return Convert.ToInt32(Math.Floor((DateOfDeath.Value.Subtract(DateOfBirth.Value).Days / 365F)));
                else if (YearOfBirth.HasValue && YearOfDeath.HasValue)
                    return YearOfDeath.Value - YearOfBirth.Value;
                return 0;
            }
        }

        public int? YearOfDeath { get; set; }

        public string Gender { get; set; }
        public bool IsSpouse { get; set; }
        public Guid ParentRelationship { get; set; }

        public List<Story> Stories { get; set; }

        public List<Gallery> Galleries { get; set; }

        public bool IsClanManager { get; set; }

        public DateTime LastUpdated { get; set; }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public Repo.Core.Model.Person MapToEntity(ClanAndPeopleService clanAndPeopleService)
        {
            return new Repo.Core.Model.Person()
            {
                Address1 = Address,
                Address2 = Address1,
                EmailAddress = EmailAddress,
                FatherID = clanAndPeopleService.People.FirstOrDefault(t => t.Id == Father.Id)?.Id,
                FirstName = FirstName,
                MiddleName = MiddleName,
                MaidenName = MaidenName,
                MotherID = clanAndPeopleService.People.FirstOrDefault(t => t.Id == Mother.Id)?.Id,
                Phone = Phone,
                LastName = LastName,
                PostCode = PostCode,
                State = State,
                SubscribeToEmail = SubscribeToEmail,
                Suburb = Suburb,
                Occupation = Occupation,
                ClandId = ClanId,
                DateOfBirth = DateOfBirth,
                YearOfBirth = YearOfBirth,
                DateOfDeath = DateOfDeath,
                YearOfDeath = YearOfDeath,
                IsSpouse = IsSpouse,
                IsClanManager = IsClanManager,
                Gender = Gender,
                ID = Id,
                ParentRelationship = ParentRelationship
            };
        }

  

        public static Person FlatMap(Repo.Core.Model.Person person, ClanAndPeopleService clanAndPeopleService)
        {
            if (person == null)
                return null;
            return new Person()
            {
                Address = person.Address1,
                Address1 = person.Address2,
                EmailAddress = person.EmailAddress,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                MaidenName = person.MaidenName,
                Phone = person.Phone,
                LastName = person.LastName,
                PostCode = person.PostCode,
                State = person.State,
                SubscribeToEmail = person.SubscribeToEmail,
                Suburb = person.Suburb,
                Occupation = person.Occupation,
                ClanName = clanAndPeopleService.Clans.FirstOrDefault(t => t.Id == person.ClandId)?.FormattedName,
                ClanId = person.ClandId,
                Id = person.ID,
                IsSpouse = person.IsSpouse,
                DateOfBirth = person.DateOfBirth,
                DateOfDeath = person.DateOfDeath,
                YearOfBirth = person.YearOfBirth,
                YearOfDeath = person.YearOfDeath,
                Gender = person.Gender,
                ParentRelationship = person.ParentRelationship,
                IsClanManager  = person.IsClanManager,
                LastUpdated = person.LastUpdated
            };
        }

    }
}
