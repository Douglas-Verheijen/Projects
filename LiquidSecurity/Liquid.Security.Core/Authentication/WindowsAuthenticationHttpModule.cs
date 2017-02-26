using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Liquid.Security.Authentication
{
    public class WindowsAuthenticationHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (context != null)
            {
                context.PostAuthenticateRequest += OnPostAuthenticateRequest;

                var formsAuthenticationModule = context.Modules["FormsAuthentication"] as FormsAuthenticationModule;
                if (formsAuthenticationModule != null)
                    formsAuthenticationModule.Authenticate += WindowsAuthenticationHttpModule_Authenticate;
            }
        }

        private void WindowsAuthenticationHttpModule_Authenticate(object sender, FormsAuthenticationEventArgs e)
        {
            if (e.Context.User == null)
            {
                if (e.Context.Request.ServerVariables["HTTP_SOAPACTION"] != null || SecurityProvider.Mode != AuthenticationMode.Windows)
                {
                    var anonymousIdentity = new GenericIdentity("", "");
                    var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);
                    e.User = anonymousPrincipal;
                }
            }
        }

        private void OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;
            var context = httpApplication.Context;
            
            if (context.User == null)
            {
                WindowsIdentity iisIdentity = context.Request.LogonUserIdentity;
                if (iisIdentity != null)
                {
                    var principal = new WindowsPrincipal(iisIdentity);
                    context.User = principal;
                }
            }
            
            if (context.Request.ServerVariables["HTTP_SOAPACTION"] != null)
                HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;
        }
    }
}
