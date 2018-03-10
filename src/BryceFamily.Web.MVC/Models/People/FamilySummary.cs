using BryceFamily.Web.MVC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BryceFamily.Web.MVC.Models.People
{
    public class FamilySummary
    {

        public FamilySummary(ClanAndPeopleService clanAndPeopleService)
        {
            NearByBirthdays = clanAndPeopleService
                                        .People
                                        .Where(person => {
                                            if (!person.DateOfBirth.HasValue || person.DateOfDeath.HasValue)
                                                return false;
                                            var daysToBirthday = GetDaysToBirthday(person);
                                            return daysToBirthday > -15 && daysToBirthday < 30;
                                        })
                                        .ToList();

            SignificantAnniversaries = clanAndPeopleService
                                            .People
                                            .SelectMany(x => x.Unions)
                                            .Where(t => !t.Divorced
                                                && (t.Partner1 != null && !t.Partner1.DateOfDeath.HasValue)
                                                && (t.Partner2 != null && !t.Partner2.DateOfDeath.HasValue)
                                                && t.DateOfUnion.HasValue
                                                && t.YearsMarried > 0
                                                && (t.YearsMarried % 10 == 0))
                                            .Distinct()
                                            .ToList();

            SignificantBirthdays = clanAndPeopleService
                                    .People
                                    .Where(person => 
                                            person.DateOfBirth.HasValue &&
                                            !person.DateOfDeath.HasValue &&
                                            person.Age == 21
                                                        || (person.Age > 21 
                                                            && person.Age % 10 == 0
                                                        ))
                                    .ToList();
        }


        public List<Person> NearByBirthdays { get; set; }

        public List<Union> SignificantAnniversaries { get; set; }

        public List<Person> SignificantBirthdays { get; set; }

        private static int GetDaysToBirthday(Person person)
        {
            return !person.DateOfBirth.HasValue
                        ? -999
                        : (int)ToBirthdayThisYear(person.DateOfBirth.Value).Subtract(DateTime.Now).TotalDays;
        }

        private static DateTime ToBirthdayThisYear(DateTime dateOfBirth)
        {
            //handle leap year babies
            if (!DateTime.IsLeapYear(DateTime.Now.Year) && dateOfBirth.Month == 2 && dateOfBirth.Day == 29)
                return new DateTime(DateTime.Now.Year, 3, 1);
            
            return new DateTime(DateTime.Now.Year, 
                                dateOfBirth.Month, 
                                dateOfBirth.Day);
        }
    }
}
