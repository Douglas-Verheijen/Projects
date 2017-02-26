using Liquid.Domain;
using Liquid.Domain.Security;
using Liquid.IoC;

namespace Liquid.Actions
{
    public interface ICreateNewActionContext<TEntity> : IFormActionContext<TEntity>
        where TEntity : Entity
    {
    }

    [GenericImplementation(typeof(ICreateNewActionContext<>))]
    class CreateNewActionContext<TEntity> : FormActionContext<TEntity>, ICreateNewActionContext<TEntity>
        where TEntity : Entity
    {        
    }

    public interface IUserCreateNewActionContext<TEntity> : ICreateNewActionContext<TEntity>
        where TEntity : User
    {
    }

    [GenericImplementation(typeof(ICreateNewActionContext<>))]
    class UserCreateNewActionContext<TEntity> : CreateNewActionContext<TEntity>, IUserCreateNewActionContext<TEntity>
        where TEntity : User
    {
    }
}
