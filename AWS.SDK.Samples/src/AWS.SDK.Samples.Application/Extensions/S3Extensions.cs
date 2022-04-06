using Amazon.S3;
using Amazon.S3.Model;
using System.Net;

namespace AWS.SDK.Samples.Application.Extensions
{
    public static class S3Extensions
    {
        public static async Task<bool> DeleteDirectoryAsync(this IAmazonS3 amazonS3, string bucketName, string path)
        {
            if (!path.EndsWith("/"))
                throw new ArgumentException("Invalid directory");

            var request = new DeleteObjectsRequest
            {
                BucketName = bucketName
            };

            var listResponse = await amazonS3.ListObjectsAsync(new ListObjectsRequest
            {
                BucketName = bucketName,
                Prefix = path
            });

            listResponse.S3Objects.ForEach(x => request.AddKey(x.Key));
            request.AddKey(path);

            var response = await amazonS3.DeleteObjectsAsync(request);

            return response.HttpStatusCode == HttpStatusCode.OK && (response.DeleteErrors == null || !response.DeleteErrors.Any());
        }
    }
}
