using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Files
{
    public interface IFileService
    {
        Task<byte[]> GetFile(Guid fileId, Guid galleryId);

        Task<string> SaveFile(Guid fileId, Guid galleryId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken);
        Task<byte[]> GetFileResized(Guid resourceId, Guid galleryId, double resizeLimit);
    }
}
