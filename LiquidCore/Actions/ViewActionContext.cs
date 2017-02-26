using Liquid.Domain;
using Liquid.IoC;

namespace Liquid.Actions
{
    public interface IViewActionContext<TEntity> : IFormActionContext<TEntity>
        where TEntity : Entity
    {
        object EntityId { get; set; }
    }

    [GenericImplementation(typeof(IViewActionContext<>))]
    class ViewActionContext<TEntity> : FormActionContext<TEntity>, IViewActionContext<TEntity>
        where TEntity : Entity
    {
        public object EntityId { get; set; }
    }
}
