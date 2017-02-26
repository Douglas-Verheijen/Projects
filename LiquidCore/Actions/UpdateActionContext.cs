using Liquid.Domain;
using Liquid.IoC;
using System;

namespace Liquid.Actions
{
    public interface IUpdateActionContext<TEntity> : IFormActionContext<TEntity>
        where TEntity : Entity
    {
        Guid EntityId { get; set; }
    }

    [GenericImplementation(typeof(IUpdateActionContext<>))]
    class UpdateActionContext<TEntity> : FormActionContext<TEntity>, IUpdateActionContext<TEntity>
        where TEntity : Entity
    {
        public Guid EntityId { get; set; }
    }
}
