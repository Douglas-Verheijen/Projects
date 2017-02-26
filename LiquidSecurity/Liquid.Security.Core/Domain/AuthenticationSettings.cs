using Liquid.Domain;
using Liquid.Security.Authentication;

namespace Liquid.Security.Domain
{
    public class AuthenticationSettings : Entity
    {
        public virtual AuthenticationMode Mode { get; set; }
    }

    public class AuthenticationSettingsMapping : EntityMap<AuthenticationSettings>
    {
        public AuthenticationSettingsMapping()
        {
            Table("AuthenticationSettings");
            Schema("Security");
            Map(x => x.Mode);
        }
    }
}
