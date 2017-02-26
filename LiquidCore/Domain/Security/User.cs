namespace Liquid.Domain.Security
{
    public class User : Entity
    {
        public virtual string Email { get; set; }

        public virtual string Firstname { get; set; }

        public virtual string Lastname { get; set; }

        public virtual string Username { get; set; }

        //public virtual bool IsActivated { get; set; }
    }

    public class UserMap : EntityMap<User>
    {
        public UserMap()
            : base()
        {
            Table("[User]");
            Schema("[Security]");
            Map(x => x.Username);
            Map(x => x.Firstname);
            Map(x => x.Lastname);
            Map(x => x.Email);
        }
    }
}
