using Liquid.Actions;
using Liquid.IoC;

namespace Liquid.Domain.Security.Actions
{
    public interface IUserDeactivationActionContext<TEntity> : IUpdateActionContext<TEntity>
        where TEntity : Entity
    {
    }

    [GenericImplementation(typeof(IUserDeactivationActionContext<>))]
    class UserDeactivationActionContext<TEntity> : UpdateActionContext<TEntity>, IUserDeactivationActionContext<TEntity>
        where TEntity : User
    {
    }
}
