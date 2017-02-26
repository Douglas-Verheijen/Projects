using Liquid.Data;
using Liquid.IoC;
using Liquid.Services;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace Liquid.Security.Authentication
{
    public class SocialMediaLoginRequest : IServiceRequest<SocialMediaLoginResponse>
    {
        public ExternalLoginInfo LoginInfo { get; set; }
    }

    public class SocialMediaLoginResponse : IServiceResponse
    {
        public SignInStatus SignInStatus { get; set; }

        public bool Success
        {
            get { return SignInStatus == SignInStatus.Success; }
            set { SignInStatus = value ? SignInStatus.Success : SignInStatus.Failure; }
        }
    }

    [DefaultImplementation(typeof(IRequestHandler<SocialMediaLoginRequest, SocialMediaLoginResponse>))]
    class SocialMediaLoginRequestHandler : RequestHandler<SocialMediaLoginRequest, SocialMediaLoginResponse>
    {
        public SocialMediaLoginRequestHandler(IDataContext dataContext)
            : base(dataContext) { }

        protected override void HandleRequest(SocialMediaLoginRequest request, SocialMediaLoginResponse response)
        {
            var loginInfo = request.LoginInfo;
            var signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();

            response.SignInStatus = signInManager.ExternalSignIn(loginInfo, isPersistent: false);
        }
    }
}
