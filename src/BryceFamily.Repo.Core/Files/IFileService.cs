﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Files
{
    public interface IFileService
    {
        Task<byte[]> GetFile(Guid fileId, Guid galleryId);

        Task<string> SaveFile(Guid fileId, Guid galleryId, IFormFile fileStream, string fileName);
        Task<byte[]> GetFileThumbnail(Guid resourceId, Guid galleryId);
    }
}