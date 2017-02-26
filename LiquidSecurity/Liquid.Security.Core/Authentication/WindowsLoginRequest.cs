using Liquid.Data;
using Liquid.IoC;
using Liquid.Security.Domain;
using Liquid.Services;
using System.Linq;
using System.Security.Principal;

namespace Liquid.Security.Authentication
{
    public class WindowsLoginRequest : IServiceRequest<WindowsLoginResponse>
    {
        public WindowsIdentity Identity { get; set; }
    }

    public class WindowsLoginResponse : IServiceResponse
    {
        public bool Success { get; set; }
    }

    [DefaultImplementation(typeof(IRequestHandler<WindowsLoginRequest, WindowsLoginResponse>))]
    class WindowsLoginRequestHandler : RequestHandler<WindowsLoginRequest, WindowsLoginResponse>
    {
        private readonly LiquidPrincipalFactory _factory;

        public WindowsLoginRequestHandler(IDataContext dataContext, LiquidPrincipalFactory factory)
            : base(dataContext)
        {
            _factory = factory;
        }

        protected override void HandleRequest(WindowsLoginRequest request, WindowsLoginResponse response)
        {
            var identity = request.Identity;
            var context = ApplicationDbContext.Create();

            var authenticatedUser = context.Users.FirstOrDefault(x => x.UserName == identity.Name);
            if (authenticatedUser == null)
            {
                response.Success = false;
                return;
            }

            identity.SetCanLogOff(false);
            _factory.Create(identity, AuthenticationMode.Federation);
            response.Success = true;
        }
    }
}
