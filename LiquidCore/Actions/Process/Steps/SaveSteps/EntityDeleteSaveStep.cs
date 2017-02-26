using Liquid.Data;
using Liquid.Domain;
using Liquid.IoC;
using System;

namespace Liquid.Actions.Process.Steps.SaveSteps
{
    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityDeleteSaveStep<TEntity, TAction> : IFormActionProcessStep<TEntity, TAction, ISaveProcess>
        where TEntity : Entity
        where TAction : IDeleteActionContext<TEntity>
    {
        private readonly IDataContext _dataContext;

        public EntityDeleteSaveStep(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Execute(TAction actionContext)
        {
            if (actionContext == null)
                throw new Exception("actionContext");

            _dataContext.Delete(actionContext.Entity);
        }
    }
}
