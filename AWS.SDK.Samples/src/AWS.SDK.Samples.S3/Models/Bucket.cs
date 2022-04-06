namespace AWS.SDK.Samples.S3.Models
{
    public class Bucket : ICloudDirectory
    {
        public Bucket(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
