using BryceFamily.Repo.Core.Read.People;
using BryceFamily.Web.MVC.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class ClanAndPeopleService
    {
        private readonly IPersonReadRepository _peopleReadRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnionReadRepository _unionReadRepository;

        public ClanAndPeopleService(IPersonReadRepository peopleReadRepository, IMemoryCache memoryCache, IUnionReadRepository unionReadRepository)
        {
            _peopleReadRepository = peopleReadRepository;
            _memoryCache = memoryCache;
            _unionReadRepository = unionReadRepository;
        }

        private readonly IReadOnlyList<string> _clans = new List<string>()
        {
            "Smith",
            "Thomas",
            "Robson",
            "Moore",
            "GBryce",
            "CBryce",
            "DBryce",
            "HBryce",
            "RBryce",
            "Shield",
            "Hose",
            "Barmby"
        };

        public IReadOnlyList<string> Clans => _clans;

        private static readonly object _peopleLock = new object();
        private static readonly object _familyTreeLock = new object();

        private const string _PEOPLELIST = "peopleList";
        private const string _FAMILYTREE = "familyTree";

        
        public IReadOnlyList<Person> People
        {
            get
            {
                // Key not in cache, so get data.
                if (!_memoryCache.TryGetValue(_PEOPLELIST, out List<Person> peopleList))
                {
                     var processedPeople = new List<Person>();
        
                    var temp = _peopleReadRepository.GetAllPeople(CancellationToken.None).Result;

                    peopleList = new List<Person>();

                    //hydrate the master list of people

                    peopleList.AddRange(temp.Select(p => Person.FlatMap(p)));

                    foreach(var person in temp)
                    {
                        MapParents(person, peopleList);
                    }

                    var processedUnions = new List<Guid>();

                    var unions = _unionReadRepository.GetAllUnions(CancellationToken.None).Result;

                    unions.ForEach(union => ProcessUnionDescendants(unions, union, temp, peopleList, processedUnions));
                  

                    //// Save data in cache.
                    _memoryCache.Set(_PEOPLELIST, peopleList);

                }
                return (IReadOnlyList<Person>) peopleList;

            }
        }

        private void MapParents(Repo.Core.Model.Person dbPerson,  List<Person> peopleList)
        {
            var person = peopleList.First(p => p.Id == dbPerson.ID);
            person.Father = peopleList.FirstOrDefault(f => f.PersonId == dbPerson.FatherID);
            person.Mother = peopleList.FirstOrDefault(f => f.PersonId == dbPerson.MotherID);
        }

        private void ProcessUnionDescendants(List<Repo.Core.Model.Union> unions, Repo.Core.Model.Union union, List<Repo.Core.Model.Person> peopleLookup, List<Person> peopleList, List<Guid> processedUnions)
        {

            if (processedUnions.Any(u => u == union.ID))
                return;

            var partner1 = peopleList.First(p => p.PersonId == union.PartnerID);
            var partner2 = peopleList.First(p => p.PersonId == union.Partner2ID);
            var children = peopleList.Where(c => c.ParentRelationship == union.ID);
            //var children = peopleLookup.Where(f => f?.ParentRelationship == union.ID);
            //make the union
            var newUnion = new Models.Union
            {
                Partner1 = partner1,
                Partner2 = partner2,
                DateOfUnion = union.MarriageDate,
                DateOfDissolution = union.DivorceDate,
                Descendents = children.ToList()
            };
            processedUnions.Add(union.ID);


            partner1.Unions.Add(newUnion);
            partner2.Unions.Add(newUnion);

        }

        public void ClearPeople()
        {
            if (_memoryCache.TryGetValue(_PEOPLELIST, out var peopleList))
                _memoryCache.Remove(_PEOPLELIST);
        }
    }
}
