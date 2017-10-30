using BryceFamily.Repo.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Read.FamilyEvents
{
    public interface IFamilyEventReadRepository
    {
        Task<List<FamilyEvent>> GetAllEventsStartingAfter(DateTime referenceDate, CancellationToken cancellation);

        Task<List<FamilyEvent>> GetAllEvents(CancellationToken cancellation);


        Task<FamilyEvent> Load(Guid id, CancellationToken cancellationToken);
    }
}
