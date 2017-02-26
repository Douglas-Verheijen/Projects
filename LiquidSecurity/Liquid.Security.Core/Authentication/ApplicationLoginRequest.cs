using Liquid.Data;
using Liquid.IoC;
using Liquid.Services;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace Liquid.Security.Authentication
{
    public class ApplicationLoginRequest : IServiceRequest<ApplicationLoginResponse>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ApplicationLoginResponse : IServiceResponse
    {
        public SignInStatus SignInStatus { get; set; }

        public bool Success
        {
            get { return SignInStatus == SignInStatus.Success; }
            set { SignInStatus = value ? SignInStatus.Success : SignInStatus.Failure; }
        }
    }

    [DefaultImplementation(typeof(IRequestHandler<ApplicationLoginRequest, ApplicationLoginResponse>))]
    class ApplicationLoginRequestHandler : RequestHandler<ApplicationLoginRequest, ApplicationLoginResponse>
    {
        public ApplicationLoginRequestHandler(IDataContext dataContext)
            : base(dataContext) { }

        protected override void HandleRequest(ApplicationLoginRequest request, ApplicationLoginResponse response)
        {   
            var signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            response.SignInStatus = signInManager.PasswordSignIn(request.Email,
                                                    request.Password, 
                                                    request.RememberMe, 
                                                    shouldLockout: false);
        }
    }
}
