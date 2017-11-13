using BryceFamily.Repo.Core.Read.People;
using BryceFamily.Web.MVC.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class ClanAndPeopleService
    {

        private readonly IPersonReadRepository _peopleReadRepository;
        private readonly IMemoryCache _memoryCache;

        public ClanAndPeopleService(IPersonReadRepository peopleReadRepository, IMemoryCache memoryCache)
        {
            _peopleReadRepository = peopleReadRepository;
            _memoryCache = memoryCache;

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



        private static readonly object _lock = new object();

        private const string _PEOPLELIST = "peopleList";
        private const string _FAMILYTREE = "familyTree";

        public IReadOnlyList<PersonLookup> People
        {
            get
            {
                if (!_memoryCache.TryGetValue(_PEOPLELIST, out var peopleList))
                {
                    // Key not in cache, so get data.
                    lock (_lock)
                    {
                        peopleList = _peopleReadRepository
                            .GetAllPeople(CancellationToken.None)
                            .Result.Select(p => new PersonLookup()
                            {
                                Clan = p.Clan,
                                FirstName = p.FirstName,
                                LastName = p.LastName,
                                ID = p.ID,
                                MotherId = p.MotherId,
                                FatherId = p.FatherId,
                                PersonId = p.PersonId
                            }).ToList();

                        // Save data in cache.
                        _memoryCache.Set(_PEOPLELIST, peopleList);
                    }

                }
                return (IReadOnlyList<PersonLookup>) peopleList;

            }
        }

        private static readonly object _familyTreeLock = new object();

        public FamilyTreeViewModel GetFamilyTreeViewModelFromRootNode(int personId, int maxLevel)
        {
            FamilyTreeViewModel tree = null;
            if (!_memoryCache.TryGetValue(_FAMILYTREE, out var familyTree))
            {
                //build new family tree from scratch
                lock (_familyTreeLock)
                {
                    var person = _peopleReadRepository.Load(personId, CancellationToken.None).Result;

                    tree = new FamilyTreeViewModel
                    {
                        FirstNames = $"{person.FirstName} {person.MiddleName}",
                        LastName = person.LastName,
                        Gender = "M",
                        Level = 1,
                        Spouses = ResolveSpouse(person),
                        Descendents = ResolveDescendents(person, maxLevel)
                    };


                }
            }
            else
            {
                //does the node exist in the stored cache? if not add it to the 
                var resolvedTree = (FamilyTreeViewModel) familyTree;

                lock (_familyTreeLock)
                {

                }

            }

            return tree;
        }

        private List<FamilyTreeViewModel> ResolveDescendents(Repo.Core.Model.Person person, int maxLevel)
        {
            var descendants = new List<FamilyTreeViewModel>();

            person.Descendants.ForEach(des =>
            {
                var descendant = People.FirstOrDefault(p => p.PersonId == des.PersonID);
                var vm = new FamilyTreeViewModel()
                {
                    FirstNames = descendant.FirstName,
                    LastName = descendant.LastName,
                    //DOB, Gender etc
                    Descendents = ResolveDescendents()
                };
                descendants.Add(vm);
            });

            return descendants;
        }

        private List<FamilyTreeViewModel> ResolveSpouse(Repo.Core.Model.Person person)
        {
            var spouses = new List<FamilyTreeViewModel>();
            if (person.Relationships.Any())
            {
                person.Relationships.ForEach(sp =>
                {
                    if (sp.HusbandID != person.PersonID)
                    {
                        var spouse = People.FirstOrDefault(t => t.PersonId == sp.HusbandID);
                        if (spouse != null)
                            spouses.Add(new FamilyTreeViewModel()
                            {
                                FirstNames = spouse.FirstName,
                                Gender = "M"
                            });
                    }
                    else
                    {
                        var spouse = People.FirstOrDefault(t => t.PersonId == sp.WifeID);
                        if (spouse != null)
                            spouses.Add(new FamilyTreeViewModel()
                            {
                                FirstNames = spouse.FirstName,
                                Gender = "F"
                            });
                    }
                        
                });
            }

            return spouses;
        }
    }
}
