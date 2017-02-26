using System.Security.Claims;
using System.Security.Principal;

namespace Liquid.Security
{
    public class LiquidIdentity : ClaimsIdentity
    {
        public LiquidIdentity(IIdentity identity)
            : base(identity) { }
    }
}
