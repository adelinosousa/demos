using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System.Net;

namespace AWS.SDK.Cognito
{
    public class UserPool
    {
        private readonly IAmazonCognitoIdentityProvider identityProvider;
        private readonly string poolName;

        public UserPool(IAmazonCognitoIdentityProvider identityProvider, string poolName)
        {
            this.identityProvider = identityProvider;
            this.poolName = poolName;
        }

        public string? Id { get; private set; }

        public async Task<bool> Create()
        {
            if (!await IsPoolNameUnique())
                return false;

            var response = await identityProvider.CreateUserPoolAsync(new CreateUserPoolRequest
            {
                PoolName = poolName,
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

            Id = response.UserPool.Id;

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> CreateClient()
        {
            if (string.IsNullOrWhiteSpace(Id))
                return false;

            var response = await identityProvider.CreateUserPoolClientAsync(new CreateUserPoolClientRequest
            {
                ClientName = $"{poolName}-client",
                UserPoolId = Id,
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

        public async Task<bool> Delete()
        {
            var response = await identityProvider.DeleteUserPoolAsync(new DeleteUserPoolRequest
            {
                UserPoolId = Id
            });

            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        private async Task<bool> IsPoolNameUnique()
        {
            var response = await identityProvider.ListUserPoolsAsync(new ListUserPoolsRequest { MaxResults = 10 });

            return !response.UserPools.Any(x => x.Name.Equals(poolName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
