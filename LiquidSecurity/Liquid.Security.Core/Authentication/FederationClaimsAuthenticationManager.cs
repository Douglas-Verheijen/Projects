using System.Linq;
using System.Security.Claims;

namespace Liquid.Security.Authentication
{
    class FederationClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal == null)
                return null;

            var principal = base.Authenticate(resourceName, incomingPrincipal);
            if (principal == null || principal.Identities == null)
                return null;

            var claimsIdentity = principal.Identities.FirstOrDefault();
            if (claimsIdentity == null)
                return null;

            var claimsIdentityType = claimsIdentity.GetType();

            var loginName = claimsIdentity.Name;
            if (string.IsNullOrEmpty(loginName))
                return null;

            var request = new FederationLoginRequest();
            request.Username = loginName;
            request.Identity = claimsIdentity;

            var response = request.Handle();
            return response.Principal;
        }
    }
}
