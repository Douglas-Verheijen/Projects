using Liquid.Data;
using Liquid.IoC;
using Liquid.Services;

namespace Liquid.Security.Authentication
{
    public class SAMLLoginRequest : IServiceRequest<SAMLLoginResponse>
    {
    }

    public class SAMLLoginResponse : IServiceResponse
    {
        public bool Success { get; set; }
    }

    [DefaultImplementation(typeof(IRequestHandler<SAMLLoginRequest, SAMLLoginResponse>))]
    class SAMLLoginRequestHandler : RequestHandler<SAMLLoginRequest, SAMLLoginResponse>
    {
        public SAMLLoginRequestHandler(IDataContext dataContext)
            : base(dataContext) { }

        protected override void HandleRequest(SAMLLoginRequest request, SAMLLoginResponse response)
        {
        }
    }
}
