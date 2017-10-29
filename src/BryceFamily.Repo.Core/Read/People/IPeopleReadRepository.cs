using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.People
{
    public interface IPersonReadRepository
    {
        Task<Person> Load(Guid id, CancellationToken cancellationToken);


        Task<List<Person>> SearchByName(string firstName, string lastName, string emailAddress, string occupation, CancellationToken cancellationToken);
    }
}
