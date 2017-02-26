//using Liquid.Actions;
//using Liquid.Actions.Process;
//using Liquid.IoC;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Liquid.Domain.Security.Actions.Steps
//{
//    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
//    class UserActivationValidationStep<TEntity, TAction> : FormActionProcessStep<TEntity, TAction, ValidateProcess>
//        where TEntity : User
//        where TAction : IUserActivationActionContext<TEntity>
//    {
//        public override void Execute(TAction actionContext)
//        {
//            if (actionContext == null)
//                throw new Exception("actionContext");

//            if (string.IsNullOrEmpty(actionContext.Entity.Firstname))
//                throw new Exception("First Name");
//        }
//    }
//}
