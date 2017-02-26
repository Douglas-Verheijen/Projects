namespace Liquid.Security.Configuration
{
    public interface IGoogleAuthenticationSettings
    {
        string ClientId { get; }

        string ClientSecret { get; }
    }

    public class GoogleAuthenticationSettings : IGoogleAuthenticationSettings
    {
        public GoogleAuthenticationSettings(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ClientId { get; private set; }

        public string ClientSecret { get; private set; }
    }
}
