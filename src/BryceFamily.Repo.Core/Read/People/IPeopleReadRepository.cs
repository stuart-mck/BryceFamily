using BryceFamily.Repo.Core.Model;
using BryceFamily.Repo.Core.Read.People.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.People
{
    public interface IPersonReadRepository
    {
        Task<Person> Load(Guid id, CancellationToken cancellationToken);

        Task<Person> Load(int personId, CancellationToken cancellationToken);

        Task<List<Person>> SearchByName(string clan, string firstName, string lastName, string emailAddress, string occupation, CancellationToken cancellationToken);
        Task<List<LightWeightPerson>> GetAllPeople(CancellationToken cancellationToken);
    }
}
