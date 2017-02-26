using Liquid.Data;
using Liquid.Domain;
using Liquid.IoC;
using System;
using System.Linq;

namespace Liquid.Actions.Process.Steps
{
    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityListInitiateStep<TEntity, TAction> : IFormActionProcessStep<TEntity, TAction, IInitiateProcess>
        where TEntity : Entity
        where TAction : IListActionContext<TEntity>
    {
        private readonly IDataContext _dataContext;

        public EntityListInitiateStep(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Execute(TAction actionContext)
        {
            if (actionContext == null)
                throw new Exception("actionContext");

            var query = _dataContext.Query<TEntity>();
            var count = query.Count();
            if (actionContext.PageSize > 0)
            {
                if (actionContext.Page > 0)
                    query = query.Skip(actionContext.Page * actionContext.PageSize);
                query = query.Take(actionContext.PageSize);
            }

            actionContext.Results = query.ToList();
            actionContext.Success = true;
            actionContext.Total = count;
        }
    }
}
