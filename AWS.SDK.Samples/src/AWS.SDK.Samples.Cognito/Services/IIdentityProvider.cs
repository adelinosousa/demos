using AWS.SDK.Samples.Cognito.Models;

namespace AWS.SDK.Samples.Cognito.Services
{
    public interface IIdentityProvider
    {
        Task<bool> Create(IIdentityTenant tenant);

        Task<bool> CreateClient(IIdentityTenant tenant);

        Task<bool> Delete(IIdentityTenant tenant);
    }
}
