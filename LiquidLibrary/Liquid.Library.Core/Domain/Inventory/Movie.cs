using Liquid.Domain;
using Liquid.Metadata;
using System.Runtime.Serialization;

namespace Liquid.Library.Domain.Inventory
{
    [DataContract]
    [InstanceDisplayName("{{Name}}")]
    public class Movie : Entity
    {
        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual MovieFormat Format { get; set; }
    }

    public enum MovieFormat
    {
        DVD,
        BluRay,
        VHS
    }

    public class MovieMap : EntityMap<Movie>
    {
        public MovieMap()
            : base()
        {
            Table("[Movie]");
            Schema("[Inventory]");
            Map(x => x.Name);
            Map(x => x.Format);
        }
    }
}
