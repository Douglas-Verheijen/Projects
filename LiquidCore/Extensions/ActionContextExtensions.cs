using Liquid.Actions;
using Liquid.Domain;
using Liquid.IoC;
using System;
using System.Linq;

namespace Liquid
{
    public static class ActionContextExtensions
    {
        public static void InitiateProcess(this IFormActionContext actionContext)
        {
            Type[] genericArgs;

            var actionContextType = actionContext.GetType();
            if (actionContextType.TryGetGenericArgumentsFromSubclassOfGenericDefinition(typeof(IFormActionContext<>), out genericArgs))
                typeof(ActionContextExtensions).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .FirstOrDefault(x => x.Name == "InitiateProcess" && x.IsGenericMethod)
                    .MakeGenericMethod(genericArgs[0], actionContextType)
                    .Invoke(null, new object[] { actionContext });
        }

        public static void InitiateProcess<TEntity, TAction>(this TAction actionContext)
            where TEntity : Entity
            where TAction : IFormActionContext<TEntity>
        {
            var processor = ConfigurationProvider.GetService<IInitiateFormActionProcessor<TEntity, TAction>>();
            processor.Run(actionContext);
        }

        public static void Process(this IFormActionContext actionContext)
        {
            Type[] genericArgs;

            var actionContextType = actionContext.GetType();
            if (actionContextType.TryGetGenericArgumentsFromSubclassOfGenericDefinition(typeof(IFormActionContext<>), out genericArgs))
                typeof(ActionContextExtensions).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .FirstOrDefault(x => x.Name == "Process" && x.IsGenericMethod)
                    .MakeGenericMethod(genericArgs[0], actionContextType)
                    .Invoke(null, new object[] { actionContext });
        }

        public static void Process<TEntity, TAction>(this TAction actionContext)
            where TEntity : Entity
            where TAction : IFormActionContext<TEntity>
        {
            var processor = ConfigurationProvider.GetService<IFormActionProcessor<TEntity, TAction>>();
            processor.Run(actionContext);
        }
    }
}
