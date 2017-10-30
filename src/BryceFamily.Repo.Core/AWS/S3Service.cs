using BryceFamily.Repo.Core.Files;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using System.Net;
using System.Threading;

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

        public async Task<string> SaveFile(Guid fileId, Guid galleryId, IFormFile fileStream, string fileName, CancellationToken cancellationToken)
        {
            var s3Context = _awsClientFactory.GetS3Context();

            //validate bucket
            //root/galleryid/fileName or root/personid/filename

            var bucketPath = $"{_bucketRoot}/{galleryId}";
            await EnsureBucketExists(s3Context, bucketPath, cancellationToken);

            //write to bucket
            var result = await s3Context.PutObjectAsync(new PutObjectRequest()
            {
                BucketName = bucketPath,
                InputStream = fileStream.OpenReadStream()
            }, cancellationToken);

            if (result.HttpStatusCode == HttpStatusCode.OK)
            {
                return $"{bucketPath}/{fileName}";
            }
            throw new Exception($"Could not save file - status code returned was {result.HttpStatusCode}");
            
        }

        private async Task EnsureBucketExists(IAmazonS3 s3Context, string bucketPath, CancellationToken cancellationToken)
        {
            if (!await s3Context.DoesS3BucketExistAsync(bucketPath))
                await s3Context.PutBucketAsync(bucketPath, cancellationToken);
        }
    }
}
