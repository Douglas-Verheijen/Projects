using Liquid.Data;
using Liquid.Domain;
using Liquid.IoC;
using System;

namespace Liquid.Actions.Process.Steps
{
    abstract class EntityFormActionSaveStep<TEntity, TAction> : IFormActionProcessStep<TEntity, TAction, ISaveProcess>
        where TEntity : Entity
        where TAction : IFormActionContext<TEntity>
    {
        private readonly IDataContext _dataContext;

        public EntityFormActionSaveStep(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Execute(TAction actionContext)
        {
            if (actionContext == null)
                throw new Exception("actionContext");

            _dataContext.Save(actionContext.Entity);
        }
    }

    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityCreateNewSaveStep<TEntity, TAction> : EntityFormActionSaveStep<TEntity, TAction>
        where TEntity : Entity
        where TAction : ICreateNewActionContext<TEntity>
    {
        public EntityCreateNewSaveStep(IDataContext dataContext)
            : base(dataContext) { }
    }

    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityUpdateSaveStep<TEntity, TAction> : EntityFormActionSaveStep<TEntity, TAction>
        where TEntity : Entity
        where TAction : IUpdateActionContext<TEntity>
    {
        public EntityUpdateSaveStep(IDataContext dataContext)
            : base(dataContext) { }
    }
}
