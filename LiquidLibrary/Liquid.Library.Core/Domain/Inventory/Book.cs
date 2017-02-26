using Liquid.Domain;
using Liquid.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Liquid.Library.Domain.Inventory
{
    [DataContract]
    [InstanceDisplayName("{{Name}}")]
    public class Book : Entity
    {
        [Required]
        [DataMember]
        public virtual string Name { get; set; }

        [Required]
        [DataMember]
        public virtual string Author { get; set; }

        [Required]
        [DataMember]
        public virtual string ISBN { get; set; }
    }

    public class BookMap : EntityMap<Book>
    {
        public BookMap()
            : base()
        {
            Table("Book");
            Schema("Inventory");
            Map(x => x.Name);
            Map(x => x.Author);
            Map(x => x.ISBN);
        }
    }
}
