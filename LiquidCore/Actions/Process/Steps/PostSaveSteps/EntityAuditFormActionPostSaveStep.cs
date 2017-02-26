using Liquid.Domain;
using Liquid.Domain.Audit.Services;
using Liquid.IoC;

namespace Liquid.Actions.Process.Steps
{
    abstract class EntityAuditFormActionPostSaveStep<TEntity, TAction> : IFormActionProcessStep<TEntity, TAction, IPostSaveProcess>
        where TEntity : Entity
        where TAction : IFormActionContext<TEntity>
    {
        private readonly IEntityChangeTrackerService _entityChangeTracker;

        public EntityAuditFormActionPostSaveStep(IEntityChangeTrackerService entityChangeTracker)
        {
            _entityChangeTracker = entityChangeTracker;
        }

        protected abstract string ActionName { get; } 

        public void Execute(TAction actionContext)
        {
            _entityChangeTracker.CreateRecords(actionContext.Entity, ActionName);
        }
    }

    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityAuditCreateNewPostSaveStep<TEntity, TAction> : EntityAuditFormActionPostSaveStep<TEntity, TAction>
        where TEntity : Entity
        where TAction : ICreateNewActionContext<TEntity>
    {
        public EntityAuditCreateNewPostSaveStep(IEntityChangeTrackerService entityChangeTracker)
            : base(entityChangeTracker) { }

        protected override string ActionName
        {
            get { return "CreateNew"; }
        }
    }

    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityAuditUpdatePostSaveStep<TEntity, TAction> : EntityAuditFormActionPostSaveStep<TEntity, TAction>
        where TEntity : Entity
        where TAction : IUpdateActionContext<TEntity>
    {
        public EntityAuditUpdatePostSaveStep(IEntityChangeTrackerService entityChangeTracker)
            : base(entityChangeTracker) { }

        protected override string ActionName
        {
            get { return "Update"; }
        }
    }
}
