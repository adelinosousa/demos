using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace AWSS3Sample
{
    public class Bucket
    {
        private readonly IAmazonS3 s3Client;
        private readonly string bucketName;

        public Bucket(IAmazonS3 s3Client, string bucketName)
        {
            this.s3Client = s3Client;
            this.bucketName = bucketName;
        }

        public async Task<bool> Create(CancellationToken cancellationToken = default)
        {
            var response = await s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            }, cancellationToken);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> Exists()
        {
            return await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        }

        public async Task<string> Location(CancellationToken cancellationToken = default)
        {
            var response = await s3Client.GetBucketLocationAsync(new GetBucketLocationRequest()
            {
                BucketName = bucketName
            }, cancellationToken);

            return response.Location.ToString();
        }

        public async Task<bool> Delete(CancellationToken cancellationToken = default)
        {
            var response = await s3Client.DeleteBucketAsync(bucketName, cancellationToken);

            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
    }
}
