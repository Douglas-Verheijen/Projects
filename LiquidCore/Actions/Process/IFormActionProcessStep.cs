using Liquid.Domain;

namespace Liquid.Actions.Process
{
    public interface IFormActionProcessStep
    { 
    }

    public interface IFormActionProcessStep<TEntity, TAction> : IFormActionProcessStep
        where TEntity : Entity
        where TAction : IFormActionContext<TEntity>
    {
        void Execute(TAction actionContext);
    }

    public interface IFormActionProcessStep<TEntity, TAction, TProcess> : IFormActionProcessStep<TEntity, TAction>
        where TEntity : Entity
        where TAction : IFormActionContext<TEntity>
        where TProcess : IFormActionProcess
    {
    }
}
