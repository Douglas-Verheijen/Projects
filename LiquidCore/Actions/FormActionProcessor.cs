using Liquid.Actions.Process;
using Liquid.Data;
using Liquid.Domain;
using Liquid.IoC;
using System;
using System.Linq;

namespace Liquid.Actions
{
    public interface IFormActionProcessor<TEntity, TAction>
        where TEntity : Entity
        where TAction : IFormActionContext<TEntity>
    {
        void Run(TAction actionContext);
    }

    [GenericImplementation(typeof(IFormActionProcessor<,>))]
    class FormActionProcessor<TEntity, TAction> : IFormActionProcessor<TEntity, TAction>
        where TEntity : Entity
        where TAction : IFormActionContext<TEntity>
    {
        private readonly IDataContext _dataContext;

        public FormActionProcessor(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Run(TAction actionContext)
        {
            var entityType = typeof(TEntity);
            var actionType = typeof(TAction);
            var processTypes = new Type[] 
            {
                typeof(IPreSaveProcess),
                typeof(IValidateProcess),
                typeof(ISaveProcess),
                typeof(IPostSaveProcess)
            };

            using (_dataContext.BeginTransaction())
            {
                foreach (var processPhaseType in processTypes)
                {
                    var processStepsType = typeof(IFormActionProcessStep<,,>).MakeGenericType(entityType, actionType, processPhaseType);
                    var processSteps = ConfigurationProvider.GetAllCompatibleInstances(processStepsType).Cast<IFormActionProcessStep<TEntity, TAction>>();
                    foreach (var processStep in processSteps)
                        processStep.Execute(actionContext);

                    if (actionContext.HasErrors())
                    {
                        _dataContext.InvalidateChanges();
                        return;
                    }
                }

                if (_dataContext.HasPendingChanges())
                    _dataContext.CommitChanges();
            }
        }
    }
}
