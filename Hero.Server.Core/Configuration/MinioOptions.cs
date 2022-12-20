namespace Hero.Server.Core.Configuration
{
    public class MinioOptions
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Bucket { get; set; }
    }
}
