using FluentNHibernate.Mapping;
using Liquid.Data;
using Liquid.Domain.Security;
using Liquid.IoC;
using Liquid.Metadata;
using System;
using System.Runtime.Serialization;

namespace Liquid.Domain
{
    [DataContract]
    public abstract class Entity : IPersistentObject
    {
        [ScaffoldColumn(false)]
        public virtual DateTimeOffset? CreatedOn { get; set; }

        [ScaffoldColumn(false)]
        public virtual User CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public virtual Guid Id { get; set; }

        [ScaffoldColumn(false)]
        public virtual DateTimeOffset? LastModifiedOn { get; set; }

        [ScaffoldColumn(false)]
        public virtual User LastModifiedBy { get; set; }

        public override string ToString()
        {
            var instanceDisplayNameProvider = ConfigurationProvider.GetService<IInstanceDisplayNameProvider>();
            return instanceDisplayNameProvider.GetDisplayName(this);
        }
    }

    public abstract class EntityMap<TEntity> : ClassMap<TEntity>
        where TEntity : Entity
    {
        public EntityMap()
        {
            Id(x => x.Id);
            Map(x => x.CreatedOn);
            References(x => x.CreatedBy);
            Map(x => x.LastModifiedOn);
            References(x => x.LastModifiedBy);
        }
    }
}
