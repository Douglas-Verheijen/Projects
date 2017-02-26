namespace Liquid.Security.Configuration
{
    public interface IFacebookAuthenticationSettings
    {
        string AppId { get; }

        string AppSecret { get; }
    }

    public class FacebookAuthenticationSettings : IFacebookAuthenticationSettings
    {
        public FacebookAuthenticationSettings(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
        }

        public string AppId { get; private set; }

        public string AppSecret { get; private set; }
    }
}
