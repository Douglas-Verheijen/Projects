using Liquid.Actions;
using Liquid.Actions.Process;
using Liquid.IoC;
using System;

namespace Liquid.Domain.Security.Actions.Steps
{
    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class UserCreateNewValidationStep<TEntity, TAction> : IFormActionProcessStep<TEntity, TAction, IValidateProcess>
        where TEntity : User
        where TAction : ICreateNewActionContext<TEntity>
    {
        public void Execute(TAction actionContext)
        {
            if (actionContext == null)
                throw new Exception("actionContext");

            if (string.IsNullOrEmpty(actionContext.Entity.Email))
                actionContext.Errors.Add("Email cannot be null");
        }
    }
}
