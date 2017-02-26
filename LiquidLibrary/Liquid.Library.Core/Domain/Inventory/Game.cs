using Liquid.Domain;
using Liquid.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Liquid.Library.Domain.Inventory
{
    [DataContract]
    [InstanceDisplayName("{{Name}}")]
    public class Game : Entity
    {
        [Required]
        [DataMember]
        public virtual string Name { get; set; }
    }

    public class GameMap : EntityMap<Game>
    {
        public GameMap()
            : base()
        {
            Table("Game");
            Schema("Inventory");
            Map(x => x.Name);
        }
    }
}
