using Liquid.Data;
using Liquid.IoC;
using Liquid.Security.Domain;
using Liquid.Services;
using System;
using System.Linq;
using System.Security.Claims;

namespace Liquid.Security.Authentication
{
    public class FederationLoginRequest : IServiceRequest<FederationLoginResponse>
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public ClaimsIdentity Identity { get; set; }
    }

    public class FederationLoginResponse : IServiceResponse
    {
        public ClaimsPrincipal Principal { get; set; }

        public bool Success
        {
            get { return Principal != null; }
            set { throw new NotImplementedException(); }
        }
    }

    [DefaultImplementation(typeof(IRequestHandler<FederationLoginRequest, FederationLoginResponse>))]
    class FederationLoginRequestHandler : RequestHandler<FederationLoginRequest, FederationLoginResponse>
    {
        private readonly LiquidPrincipalFactory _factory;

        public FederationLoginRequestHandler(IDataContext dataContext, LiquidPrincipalFactory factory)
            : base(dataContext)
        {
            _factory = factory;
        }

        protected override void HandleRequest(FederationLoginRequest request, FederationLoginResponse response)
        {
            var context = ApplicationDbContext.Create();
            var authenticatedUser = context.Users.FirstOrDefault(x => x.UserName == request.Username);
            if (authenticatedUser != null)
            {
                var identity = request.Identity;
                identity.SetCanLogOff(false);

                response.Principal = _factory.Create(identity, AuthenticationMode.Federation);
            }
        }
    }
}
