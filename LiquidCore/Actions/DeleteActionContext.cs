using Liquid.Domain;
using Liquid.IoC;
using System;

namespace Liquid.Actions
{
    public interface IDeleteActionContext<TEntity> : IFormActionContext<TEntity>
        where TEntity : Entity
    {
        Guid EntityId { get; set; }
    }

    [GenericImplementation(typeof(IDeleteActionContext<>))]
    class DeleteActionContext<TEntity> : FormActionContext<TEntity>, IDeleteActionContext<TEntity>
        where TEntity : Entity
    {
        public Guid EntityId { get; set; }
    }
}
