using Liquid.Domain.Security;
using System;

namespace Liquid.Data
{
    public interface IPersistentObject
    {
        Guid Id { get; set; }
        DateTimeOffset? CreatedOn { get; set; }
        User CreatedBy { get; set; }
        DateTimeOffset? LastModifiedOn { get; set; }
        User LastModifiedBy { get; set; }
    }
}
