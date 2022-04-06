namespace AWS.SDK.Samples.Cognito.Models
{
    public class UserPool : IIdentityTenant
    {
        public UserPool(string poolName)
        {
            Name = poolName;
        }

        public string Name { get; }

        public string? Id { get; set; }
    }
}
