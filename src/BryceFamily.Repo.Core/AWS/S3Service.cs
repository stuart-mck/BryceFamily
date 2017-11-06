using BryceFamily.Repo.Core.Files;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using System.Net;
using System.Threading;
using System.IO;

namespace BryceFamily.Repo.Core.AWS
{
    public class S3Service : IFileService
    {
        private readonly IAWSClientFactory _awsClientFactory;
        private readonly string _bucketRoot;

        public S3Service(IAWSClientFactory awsClientFactory, string bucketRoot)
        {
            _awsClientFactory = awsClientFactory;
            _bucketRoot = bucketRoot;
        }

        public Task<byte[]> GetFile(Guid fileId, Guid galleryId)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetFileResized(Guid resourceId, Guid galleryId, double resizeLimit)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SaveFile(Guid fileId, string galleryId, Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken)
        {
            var s3Context = _awsClientFactory.GetS3Context();

            //validate bucket
            //root/galleryid/fileName or root/personid/filename

            var bucketPath = $"{_bucketRoot}/{galleryId}";
            //EnsureBucketExists(s3Context, bucketPath, cancellationToken);

            //write to bucket
            var result = await s3Context.PutObjectAsync(new PutObjectRequest()
            {
                CannedACL = S3CannedACL.PublicRead,
                AutoCloseStream = false,
                BucketName = bucketPath,
                ContentType = contentType,
                InputStream = fileStream,
                Key = fileName,
            }, cancellationToken);

            if (result.HttpStatusCode == HttpStatusCode.OK)
            {
                return $"{bucketPath}]";
            }
            throw new Exception($"Could not save file - status code returned was {result.HttpStatusCode}");
            
        }

        public async Task SaveFile(Guid fileId, string galleryId, byte[] contents, string fileName, string contentType, CancellationToken cancellationToken)
        {
            await SaveFile(fileId, galleryId, new MemoryStream(contents), fileName, contentType, cancellationToken);
        }

        private void EnsureBucketExists(IAmazonS3 s3Context, string bucketPath, CancellationToken cancellationToken)
        {
            try
            {
                if (!s3Context.DoesS3BucketExistAsync(bucketPath).Result)
                {
                    var result = s3Context.PutBucketAsync(bucketPath, cancellationToken).Result;
                    if (result.HttpStatusCode != HttpStatusCode.OK)
                        throw new Exception($"Could not create location to save photo status code {result.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
