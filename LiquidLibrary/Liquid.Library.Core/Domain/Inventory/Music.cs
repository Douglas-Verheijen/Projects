using Liquid.Domain;
using Liquid.Metadata;
using System.ComponentModel;

namespace Liquid.Library.Domain.Inventory
{
    [InstanceDisplayName("{{AlbumName}}")]
    public class Music : Entity
    {
        [DisplayName("Album Name")]
        public virtual string AlbumName { get; set; }

        [DisplayName("Artist Name")]
        public virtual string ArtistName { get; set; }

        public virtual MusicFormat Format { get; set; }
    }

    public enum MusicFormat
    {
        CD,
        Digital,
        Record
    }

    public class MusicMap : EntityMap<Music>
    {
        public MusicMap()
            : base()
        {
            Table("Music");
            Schema("Inventory");
            Map(x => x.AlbumName);
            Map(x => x.ArtistName);
            Map(x => x.Format);
        }
    }
}
