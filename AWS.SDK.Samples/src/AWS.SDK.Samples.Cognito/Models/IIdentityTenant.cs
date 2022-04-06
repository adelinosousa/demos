namespace AWS.SDK.Samples.Cognito.Models
{
    public interface IIdentityTenant
    {
        public string Name { get; }

        public string? Id { get; set; }
    }
}
