using System;
using System.Linq;
using System.Security.Claims;

namespace Liquid.Security
{
    public static class ClaimsIdentityExtensions
    {
        private const string issuer = "LOCAL AUTHORITY";

        private static Func<Claim, bool> CanLogOffExpression
        {
            get { return x => x.Type == CustomClaimTypes.CanLogOff && x.Issuer == issuer; }
        }

        public static bool CanLogOff(this ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var claim = identity.Claims.FirstOrDefault(CanLogOffExpression);
                if (claim != null)
                {
                    bool canLogOff;
                    if (bool.TryParse(claim.Value, out canLogOff))
                        return canLogOff;
                }
            }

            return true;
        }

        public static void SetCanLogOff(this ClaimsIdentity identity, bool canLogOff)
        {
            if (identity != null)
            {
                var claim = identity.Claims.FirstOrDefault(CanLogOffExpression);
                if (claim != null)
                    identity.RemoveClaim(claim);

                claim = new Claim(CustomClaimTypes.CanLogOff, canLogOff.ToString(), ClaimValueTypes.Boolean, issuer, issuer, identity);
                identity.AddClaim(claim);
            }
        }
    }
}
