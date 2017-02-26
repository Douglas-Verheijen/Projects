namespace Liquid.Security.Configuration
{
    public interface ITwitterAuthenticationSettings
    {
        string ConsumerKey { get; }

        string ConsumerSecret { get; }

        string[] Certificates { get; set; }
    }

    public class TwitterAuthenticationSettings : ITwitterAuthenticationSettings
    {
        public TwitterAuthenticationSettings(string consumerKey, string consumerSecret, string[] certificates)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            Certificates = certificates;
        }

        public string ConsumerKey { get; }

        public string ConsumerSecret { get; }

        public string[] Certificates { get; set; }
    }
}
