using Liquid.Domain.Security;
using System;

namespace Liquid.Domain.Audit
{
    public class AuditChange : Entity
    {
        public virtual Guid? EntityId { get; set; }

        public virtual string EntityClrType { get; set; }

        public virtual string ActionName { get; set; }

        public virtual User ModifiedBy { get; set; }

        public virtual DateTimeOffset ModifiedOn { get; set; }

        public virtual string NewValue { get; set; }

        public virtual string OriginalValue { get; set; }

        public virtual string PropertyName { get; set; }
    }

    //public class AuditChangeMap : EntityMap<AuditChange>
    //{
    //    public AuditChangeMap()
    //        : base()
    //    {
    //        Table("AuditChange");
    //        Schema("Audit");
    //        Map(x => x.EntityId);
    //        Map(x => x.EntityClrType);
    //        Map(x => x.ActionName);
    //        References(x => x.ModifiedBy);
    //        Map(x => x.ModifiedOn);
    //        Map(x => x.NewValue);
    //        Map(x => x.OriginalValue);
    //        Map(x => x.PropertyName);
    //    }
    //}
}
