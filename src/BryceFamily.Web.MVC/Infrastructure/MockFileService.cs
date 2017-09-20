using BryceFamily.Repo.Core.Files;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public class MockFileService : IFileService
    {
        private readonly string _pathRoot;

        public MockFileService(string pathRoot)
        {
            _pathRoot = pathRoot;
        }
        public async Task<byte[]> GetFile(Guid fileId, Guid galleryId)
        {
            var directoryInfo = new DirectoryInfo(_pathRoot);

            if (!directoryInfo.Exists)
                return null;

            var dirPath = Path.Combine(_pathRoot, galleryId.ToString());

            if (!Directory.Exists(dirPath))
                return null;

            var fullPath = Path.Combine(dirPath, fileId.ToString());


            if (!File.Exists(fullPath))
                return null;

            byte[] fileData = null;

            using (var inputStream = File.OpenRead(fullPath))
            {
                await inputStream.ReadAsync(fileData, 0, (int)inputStream.Length);
            }
            return fileData;
        }

        public async Task<string> SaveFile(Guid fileId, Guid galleryId, IFormFile fileContent, string fileName)
        {
            var path = Path.Combine(_pathRoot, "images", "galleries",  galleryId.ToString());
                 if (!Directory.Exists(path))
                     Directory.CreateDirectory(path);

            using (var outputStream = new FileStream(Path.Combine(path, fileId.ToString()), FileMode.Create))
            {
                await fileContent.CopyToAsync(outputStream);
                outputStream.Close();
            }

            return $"images/{galleryId}/{fileId}";
        }
    }
}
