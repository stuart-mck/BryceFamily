using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.People
{
    public interface IUnionReadRepository
    {
        Task<List<Union>> GetAllUnions(CancellationToken cancellationToken);
      
        Task<Union> Load(Guid id, CancellationToken cancellationToken);
    }
}
