using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AWS.SDK.Samples.Cognito.Models;
using System.Net;

namespace AWS.SDK.Samples.Cognito.Services
{
    public class CognitoService : IIdentityProvider
    {
        private readonly IAmazonCognitoIdentityProvider identityProvider;

        public CognitoService(IAmazonCognitoIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
        }

        public async Task<bool> Create(IIdentityTenant tenant)
        {
            if (!await IsPoolNameUnique(tenant))
                return false;

            var response = await identityProvider.CreateUserPoolAsync(new CreateUserPoolRequest
            {
                PoolName = tenant.Name,
                UsernameAttributes = { "email" },
                Schema = {
                    new SchemaAttributeType
                    {
                        Name = "email",
                        Mutable = false,
                        Required = true
                    }
                },
                UsernameConfiguration = new UsernameConfigurationType
                {
                    CaseSensitive = false
                },
                AdminCreateUserConfig = new AdminCreateUserConfigType
                {
                    AllowAdminCreateUserOnly = true
                },
                AccountRecoverySetting = new AccountRecoverySettingType
                {
                    RecoveryMechanisms = { new RecoveryOptionType { Name = RecoveryOptionNameType.Admin_only, Priority = 1 } }
                }
            });

            tenant.Id = response.UserPool.Id;

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> CreateClient(IIdentityTenant tenant)
        {
            if (string.IsNullOrWhiteSpace(tenant.Id))
                return false;

            var response = await identityProvider.CreateUserPoolClientAsync(new CreateUserPoolClientRequest
            {
                ClientName = $"{tenant.Name}-client",
                UserPoolId = tenant.Id,
                RefreshTokenValidity = 30,
                AccessTokenValidity = 5,
                IdTokenValidity = 5,
                TokenValidityUnits = new TokenValidityUnitsType
                {
                    AccessToken = "minutes",
                    IdToken = "minutes",
                    RefreshToken = "days"
                },
                ExplicitAuthFlows = { "ALLOW_ADMIN_USER_PASSWORD_AUTH", "ALLOW_REFRESH_TOKEN_AUTH" }
            });

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Delete(IIdentityTenant tenant)
        {
            var response = await identityProvider.DeleteUserPoolAsync(new DeleteUserPoolRequest
            {
                UserPoolId = tenant.Id
            });

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        private async Task<bool> IsPoolNameUnique(IIdentityTenant tenant)
        {
            var response = await identityProvider.ListUserPoolsAsync(new ListUserPoolsRequest { MaxResults = 10 });

            return !response.UserPools.Any(x => x.Name.Equals(tenant.Name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
