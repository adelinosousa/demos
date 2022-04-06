using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using AWS.SDK.Samples.S3.Models;

namespace AWS.SDK.Samples.S3.Services
{
    public class S3Service : ICloudStorage
    {
        private readonly IAmazonS3 s3Client;

        public S3Service(IAmazonS3 s3Client)
        {
            this.s3Client = s3Client;
        }

        public async Task<bool> Create(ICloudDirectory cloudDirectory, CancellationToken cancellationToken = default)
        {
            var response = await s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = cloudDirectory.Name,
                UseClientRegion = true
            }, cancellationToken);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> Exists(ICloudDirectory cloudDirectory)
        {
            return await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, cloudDirectory.Name);
        }

        public async Task<string> Location(ICloudDirectory cloudDirectory, CancellationToken cancellationToken = default)
        {
            var response = await s3Client.GetBucketLocationAsync(new GetBucketLocationRequest()
            {
                BucketName = cloudDirectory.Name
            }, cancellationToken);

            return response.Location.ToString();
        }

        public async Task<bool> Delete(ICloudDirectory cloudDirectory, CancellationToken cancellationToken = default)
        {
            var response = await s3Client.DeleteBucketAsync(cloudDirectory.Name, cancellationToken);

            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
