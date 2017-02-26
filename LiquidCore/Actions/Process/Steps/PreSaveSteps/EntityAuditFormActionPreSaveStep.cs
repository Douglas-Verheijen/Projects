using Liquid.Domain;
using Liquid.Domain.Audit.Services;
using Liquid.IoC;

namespace Liquid.Actions.Process.Steps.SaveSteps
{
    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityAuditFormActionPreSaveStep<TEntity, TAction> : IFormActionProcessStep<TEntity, TAction, IPreSaveProcess>
        where TEntity : Entity
        where TAction : IFormActionContext<TEntity>
    {
        private readonly IEntityChangeTrackerService _entityChangeTracker;

        public EntityAuditFormActionPreSaveStep(IEntityChangeTrackerService entityChangeTracker)
        {
            _entityChangeTracker = entityChangeTracker;
        }

        public void Execute(TAction actionContext)
        {
            _entityChangeTracker.TrackProperties(actionContext.Entity);
        }
    }
}
