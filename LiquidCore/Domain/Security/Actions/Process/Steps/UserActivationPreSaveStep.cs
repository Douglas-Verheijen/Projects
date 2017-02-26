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
//    class UserActivationPreSaveStep<TEntity, TAction> : FormActionProcessStep<TEntity, TAction, PreSaveProcess>
//        where TEntity : User
//        where TAction : IUserActivationActionContext<TEntity>
//    {
//        public override void Execute(TAction actionContext)
//        {
//            if (actionContext == null)
//                throw new Exception("actionContext");

//            actionContext.Entity.IsActivated = true;
//        }
//    }
//}
