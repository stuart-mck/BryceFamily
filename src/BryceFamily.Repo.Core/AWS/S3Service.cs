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

        public string SaveFile(Guid fileId, Guid galleryId, IFormFile fileStream, string fileName, CancellationToken cancellationToken)
        {
            var s3Context = _awsClientFactory.GetS3Context();

            //validate bucket
            //root/galleryid/fileName or root/personid/filename

            var bucketPath = $"{_bucketRoot}/{galleryId}";
           EnsureBucketExists(s3Context, bucketPath, cancellationToken);

            //write to bucket
            var result = s3Context.PutObjectAsync(new PutObjectRequest()
            {
                BucketName = bucketPath,
                InputStream = fileStream.OpenReadStream()
            }, cancellationToken).Result;

            if (result.HttpStatusCode == HttpStatusCode.OK)
            {
                return $"{bucketPath}/{fileName}";
            }
            throw new Exception($"Could not save file - status code returned was {result.HttpStatusCode}");
            
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
