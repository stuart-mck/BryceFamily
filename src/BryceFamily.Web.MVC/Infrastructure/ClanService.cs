using BryceFamily.Repo.Core.Read.People;
using BryceFamily.Repo.Core.Read.Story;
using BryceFamily.Web.MVC.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class ClanAndPeopleService
    {
        private readonly ILogger<ClanAndPeopleService> _logger;
        private readonly IPersonReadRepository _peopleReadRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnionReadRepository _unionReadRepository;
        private readonly IStoryReadRepository _storyReadRepository;

        public ClanAndPeopleService(ILogger<ClanAndPeopleService> logger, IPersonReadRepository peopleReadRepository, IMemoryCache memoryCache, IUnionReadRepository unionReadRepository, IStoryReadRepository storyReadRepository)
        {
            this._logger = logger;
            _peopleReadRepository = peopleReadRepository;
            _memoryCache = memoryCache;
            _unionReadRepository = unionReadRepository;
            _storyReadRepository = storyReadRepository;
        }

        private readonly IReadOnlyList<FamilyClan> _clans = new List<FamilyClan>
        {
            new FamilyClan(1,"Bryce", "David and Maria"),
            new FamilyClan(2, "Bryce", "George (Lindsay) and Dorothy"),
            new FamilyClan(3, "Barmby", "Ethyl (Sissy) and George"),
            new FamilyClan(4, "Bryce", "Harold and Phyllis"),
            new FamilyClan(5, "Hose", "Thelma and Harold"),
            new FamilyClan(6, "Moore", "Eveleyn and Thomas"),
            new FamilyClan(7, "Bryce", "Roy and Phyllis"),
            new FamilyClan(8, "Bryce", "David and Edna"),
            new FamilyClan(9, "Smith", "Dorothy and Marty"),
            new FamilyClan(10, "Shield", "Hazel and William"),
            new FamilyClan(11, "Thomas", "Iris and Frank"),
            new FamilyClan(12, "Bryce", "Desmond and Marjorie"),
            new FamilyClan(13, "Robson", "Ivan"),
            new FamilyClan(14, "Bryce", "Cecil and Val"),


            /*
             * 
             * 1	Bryce	David and Maria
2	GBryce	George (Lindsay) and Dorothy
3	Barmby	Ethyl (Sissy) and George
4	HBryce	Harold and Phyllis
5	Hose	Thelma and Harold
6	Moore	Evelyn and Thomas
7	RBryce	Roy and Phyllis
8	Bryce	Edna
9	Smith	Dorothy and Marty
10	Shield	Hazel and William
11	Thomas	Iris and Frank
12	DBryce	Desmond and Marjorie
13	Robson	Shirley and Benny
14	CBryce	Cecil and Val

             * */
        };

        public IReadOnlyList<FamilyClan> Clans => _clans;

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
                    var cancellationToken = CancellationToken.None;
                     var processedPeople = new List<Person>();
        
                    var temp = _peopleReadRepository.GetAllPeople(cancellationToken).Result;

                    peopleList = new List<Person>();

                    //hydrate the master list of people

                    peopleList.AddRange(temp.Select(p => Person.FlatMap(p, this)));

                    foreach(var person in temp)
                    {
                        MapParents(person, peopleList);
                    }

                    var processedUnions = new List<Guid>();

                    var unions = _unionReadRepository.GetAllUnions(cancellationToken).Result;

                    unions.ForEach(union => ProcessUnionDescendants(unions, union, temp, peopleList, processedUnions));

                    try
                    {
                        var stories = _storyReadRepository.GetStories(cancellationToken).Result;

                        foreach (var story in stories.Where(t => t.PersonID.HasValue))
                        {
                            var person = peopleList.FirstOrDefault(p => p.Id == story.PersonID);
                            person.Stories.Add(Story.MapToIndex(story));
                        }
                    }
                    catch (AggregateException ex)
                    {
                        _logger.LogError("Failed when building people list", ex);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Failed when building people list", ex);
                        throw;
                    }

                  
                    //// Save data in cache.
                    _memoryCache.Set(_PEOPLELIST, peopleList);

                }
                return (IReadOnlyList<Person>) peopleList;

            }
        }

        private void MapParents(Repo.Core.Model.Person dbPerson,  List<Person> peopleList)
        {
            var person = peopleList.First(p => p.Id == dbPerson.ID);
            person.Father = peopleList.FirstOrDefault(f => f.Id == dbPerson.FatherID);
            person.Mother = peopleList.FirstOrDefault(f => f.Id == dbPerson.MotherID);
        }

        private void ProcessUnionDescendants(List<Repo.Core.Model.Union> unions, Repo.Core.Model.Union union, List<Repo.Core.Model.Person> peopleLookup, List<Person> peopleList, List<Guid> processedUnions)
        {

            if (processedUnions.Any(u => u == union.ID))
                return;

            var partner1 = peopleList.First(p => p.Id == union.PartnerID);
            var partner2 = peopleList.First(p => p.Id == union.Partner2ID);
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
