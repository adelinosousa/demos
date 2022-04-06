using AWS.SDK.Samples.S3.Models;

namespace AWS.SDK.Samples.S3.Services
{
    public interface ICloudStorage
    {
        Task<bool> Create(ICloudDirectory cloudDirectory, CancellationToken cancellationToken = default);

        Task<bool> Exists(ICloudDirectory cloudDirectory);

        Task<string> Location(ICloudDirectory cloudDirectory, CancellationToken cancellationToken = default);

        Task<bool> Delete(ICloudDirectory cloudDirectory, CancellationToken cancellationToken = default);
    }
}
