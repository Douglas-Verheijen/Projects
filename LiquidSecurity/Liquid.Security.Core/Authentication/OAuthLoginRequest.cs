using Liquid.Data;
using Liquid.IoC;
using Liquid.Services;

namespace Liquid.Security.Authentication
{
    //http://term.ie/oauth/example/index.php
    public class OAuthLoginRequest : IServiceRequest<OAuthLoginResponse>
    {
    }

    public class OAuthLoginResponse : IServiceResponse
    {
        public bool Success { get; set; }
    }

    [DefaultImplementation(typeof(IRequestHandler<OAuthLoginRequest, OAuthLoginResponse>))]
    class OAuthLoginRequestHandler : RequestHandler<OAuthLoginRequest, OAuthLoginResponse>
    {
        public OAuthLoginRequestHandler(IDataContext dataContext)
            : base(dataContext) { }

        protected override void HandleRequest(OAuthLoginRequest request, OAuthLoginResponse response)
        {
        }
    }
}
