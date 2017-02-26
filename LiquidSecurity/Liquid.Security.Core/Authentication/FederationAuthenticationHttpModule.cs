using System;
using System.Globalization;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Liquid.Security.Authentication
{
    public class FederationAuthenticationHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (FederatedAuthentication.WSFederationAuthenticationModule != null)
            {
                FederatedAuthentication.WSFederationAuthenticationModule.RedirectingToIdentityProvider +=
                    FederationAuthenticationHttpModule_RedirectingToIdentityProvider;

                FederatedAuthentication.SessionAuthenticationModule.SessionSecurityTokenReceived +=
                    FederationAuthenticationHttpModule_SessionSecurityTokenReceived;

                FederatedAuthentication.FederationConfigurationCreated +=
                    FederatedAuthentication_FederationConfigurationCreated;
            }

            if (context != null)
            {
                context.PostAuthenticateRequest += OnPostAuthenticateRequest;
                context.Error += context_Error;
            }
        }

        private void FederatedAuthentication_FederationConfigurationCreated(object sender, FederationConfigurationCreatedEventArgs e)
        {
            e.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager = new FederationClaimsAuthenticationManager();
        }

        private void FederationAuthenticationHttpModule_RedirectingToIdentityProvider(object sender, RedirectingToIdentityProviderEventArgs e)
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_SOAPACTION"] != null || SecurityProvider.Mode != AuthenticationMode.Federation)
            {
                e.Cancel = true;
                return;
            }

            var absoluteUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            var applicationUrl = absoluteUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? 
                                    absoluteUrl : string.Format(CultureInfo.InvariantCulture, @"{0}/", absoluteUrl);
            
            e.SignInRequestMessage.Reply = applicationUrl;

            var audienceUris = FederatedAuthentication.FederationConfiguration
                .IdentityConfiguration
                .AudienceRestriction
                .AllowedAudienceUris;

            if (audienceUris != null && !audienceUris.Any(x => x.AbsoluteUri == applicationUrl))
                audienceUris.Add(new Uri(e.SignInRequestMessage.Reply));

            //var signInParams = HttpUtility.ParseQueryString(tenantWSFederationConfiguration.SignInQueryString);
            //foreach (var key in signInParams.AllKeys)
            //    e.SignInRequestMessage.Parameters.Add(key, signInParams[key]);

            //if (!String.IsNullOrEmpty(e.SignInRequestMessage.Context))
            //{
            //    var context = HttpUtility.ParseQueryString(e.SignInRequestMessage.Context);

            //    if (context.AllKeys.Contains("ru", StringComparer.OrdinalIgnoreCase))
            //    {
            //        var ru = context.Get("ru");

            //        if (!String.IsNullOrEmpty(ru) && ru.IndexOf('?') == -1 && !ru.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            //        {
            //            context.Set("ru", ru + "/");
            //            e.SignInRequestMessage.Context = String.Join("&", context.AllKeys.Select(key =>
            //                String.Format(CultureInfo.InvariantCulture, "{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(context.Get(key))))
            //                .ToArray());
            //        }
            //    }
            //}
        }

        private void FederationAuthenticationHttpModule_SessionSecurityTokenReceived(object sender, SessionSecurityTokenReceivedEventArgs e)
        {
            if (e.SessionToken.ClaimsPrincipal != null)
            {
                //Add support for sliding expiration for the session authentication cookie for forms login
                if (e.SessionToken.ClaimsPrincipal.Identity.AuthenticationType == "Forms")
                {
                    if (FormsAuthentication.SlidingExpiration)
                    {
                        DateTime now = DateTime.UtcNow;
                        DateTime validFrom = e.SessionToken.ValidFrom;
                        DateTime validTo = e.SessionToken.ValidTo;

                        if ((now < validTo) && (now > validFrom.AddTicks((validTo - validFrom).Ticks / 2)))
                        {
                            SessionAuthenticationModule sam = sender as SessionAuthenticationModule;

                            e.SessionToken = sam.CreateSessionSecurityToken(
                                e.SessionToken.ClaimsPrincipal,
                                e.SessionToken.Context,
                                now,
                                now.Add(FormsAuthentication.Timeout),
                                e.SessionToken.IsPersistent);

                            e.ReissueCookie = true;
                        }
                    }
                }
            }
        }

        private void OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            if (FederatedAuthentication.WSFederationAuthenticationModule != null && FederatedAuthentication.WSFederationAuthenticationModule.PassiveRedirectEnabled)
            {
                if (FederatedAuthentication.WSFederationAuthenticationModule.PassiveRedirectEnabled)
                {
                    var httpApplication = (HttpApplication)sender;
                    var context = httpApplication.Context;

                    context.Response.SuppressFormsAuthenticationRedirect = true;
                }
            }
        }

        private void context_Error(object sender, EventArgs e)
        {
            var httpContext = HttpContext.Current;
            var lastError = httpContext.Server.GetLastError() as FederationException;

            if (lastError != null && lastError.Message.StartsWith("ID3206", StringComparison.OrdinalIgnoreCase))
            {
                WSFederationMessage wSFederationMessage = WSFederationMessage.CreateFromFormPost(new HttpRequestWrapper(httpContext.Request));

                if (wSFederationMessage != null && wSFederationMessage.Context != null)
                {
                    var nameValueCollection = HttpUtility.ParseQueryString(wSFederationMessage.Context);

                    var returnUrl = nameValueCollection["ru"];
                    if (returnUrl != null)
                    {
                        var uri = HttpContext.Current.Request.Url;
                        var urlPath = uri.GetLeftPart(UriPartial.Path);
                        
                        if (string.IsNullOrEmpty(uri.Query) && !urlPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                        {
                            var newReturnUrl = urlPath + "/" + uri.Query;
                            httpContext.Response.Redirect(newReturnUrl);
                        }
                    }
                }
            }
        }
    }
}
