using Liquid.Data;
using Liquid.Domain;
using Liquid.IoC;
using System;

namespace Liquid.Actions.Process.Steps.InitiateSteps
{
    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityUpdateInitiateStep<TEntity, TAction> : IFormActionProcessStep<TEntity, TAction, IInitiateProcess>
        where TEntity : Entity
        where TAction : IUpdateActionContext<TEntity>
    {
        private readonly IDataContext _dataContext;

        public EntityUpdateInitiateStep(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Execute(TAction actionContext)
        {
            if (actionContext == null)
                throw new Exception("actionContext");

            var id = actionContext.EntityId;
            actionContext.Entity = _dataContext.Load<TEntity>(id);
        }
    }
}
