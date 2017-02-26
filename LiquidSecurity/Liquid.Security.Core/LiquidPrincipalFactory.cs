using Liquid.Security.Authentication;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace Liquid.Security
{
    class LiquidPrincipalFactory
    {
        public LiquidPrincipal Create(IIdentity original, AuthenticationMode mode)
        {
            var identity = new LiquidIdentity(original);
            var principal = new LiquidPrincipal(identity);

            principal.Mode = mode;

            HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;

            return principal;
        }
    }
}
