using Liquid.Security.Authentication;
using System.Security.Claims;

namespace Liquid.Security
{
    public class LiquidPrincipal : ClaimsPrincipal
    {
        public LiquidPrincipal(LiquidIdentity identity)
            : base(identity) { }
        
        public AuthenticationMode Mode { get; set; }
    }
}
