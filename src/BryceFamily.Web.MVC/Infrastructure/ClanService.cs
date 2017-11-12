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

        

        private static object _lock = new object();

        private const string _PEOPLELIST = "peopleList";

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
                                                ID = p.ID
                                            }).ToList();
                        
                        // Save data in cache.
                        _memoryCache.Set(_PEOPLELIST, peopleList);
                    }
                    
                }
                return (IReadOnlyList<PersonLookup>)peopleList;

            }
        }
    }
}
