using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Emails
{
    public interface ISesService
    {
        Task SendEmail(string emailaddress, string message, string subject, CancellationToken cancellationToken);
        Task SendBulkEmail(IEnumerable<string> contactList, string message, string subject, CancellationToken cancellationToken);
    }
}
