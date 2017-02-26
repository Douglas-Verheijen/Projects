using Liquid.Actions;
using Liquid.IoC;

namespace Liquid.Domain.Security.Actions
{
    public interface IUserActivationActionContext<TEntity> : IUpdateActionContext<TEntity>
        where TEntity : Entity
    {
    }

    [GenericImplementation(typeof(IUserActivationActionContext<>))]
    class UserActivationActionContext<TEntity> : UpdateActionContext<TEntity>, IUserActivationActionContext<TEntity>
        where TEntity : User
    {
    }
}
