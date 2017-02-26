using Liquid.Components;
using Liquid.Components.DisplayComponents;
using Liquid.Components.EditComponents;
using Liquid.Data;
using Liquid.Domain;
using Liquid.Domain.Audit.Services;
using Liquid.IoC;
using Liquid.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.Actions.Process.Steps
{
    abstract class EntityFieldComponentFormInitiateStep<TEntity, TAction> : IFormActionProcessStep<TEntity, TAction, IInitiateProcess>
        where TEntity : Entity
        where TAction : IFormActionContext<TEntity>
    {
        public virtual void Execute(TAction actionContext)
        {
            if (actionContext == null)
                throw new Exception("actionContext");
        }
    }

    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityFieldComponentViewInitiateStep<TEntity, TAction> : EntityFieldComponentFormInitiateStep<TEntity, TAction>
        where TEntity : Entity
        where TAction : IViewActionContext<TEntity>
    {
        public override void Execute(TAction actionContext)
        {
            base.Execute(actionContext);
            foreach (var property in typeof(TEntity).GetProperties())
            {
                if (!property.GetCustomAttributes<ScaffoldColumnAttribute>().Any(x => !x.Value))
                {
                    var component = new LabelComponent();
                    component.DisplayName = property.Name;
                    component.Type = property.PropertyType;
                    component.Value = property.GetValue(actionContext.Entity);
                    actionContext.FieldComponents.Add(component);
                }
            }
        }
    }

    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityFieldComponentCreateNewInitiateStep<TEntity, TAction> : EntityFieldComponentFormInitiateStep<TEntity, TAction>
        where TEntity : Entity
        where TAction : ICreateNewActionContext<TEntity>
    {
        private readonly IFieldComponentProvider _provider;

        public EntityFieldComponentCreateNewInitiateStep(IFieldComponentProvider provider)
        {
            _provider = provider;
        }

        public override void Execute(TAction actionContext)
        {
            base.Execute(actionContext);
            foreach (var property in typeof(TEntity).GetProperties())
            {
                if (!property.GetCustomAttributes<ScaffoldColumnAttribute>().Any(x => !x.Value))
                {
                    var componentType = _provider.GetFieldComponent(property.PropertyType);
                    if (componentType != null)
                    {
                        var instance = ConfigurationProvider.GetService(componentType) as IFieldComponent;
                        if (instance != null)
                        {
                            instance.DisplayName = property.Name;
                            instance.Type = property.PropertyType;
                            instance.SetValue(string.Empty);
                            actionContext.FieldComponents.Add(instance);
                        }
                    }
                }
            }
        }
    }

    [GenericImplementation(typeof(IFormActionProcessStep<,,>))]
    class EntityFieldComponentUpdateInitiateStep<TEntity, TAction> : EntityFieldComponentFormInitiateStep<TEntity, TAction>
        where TEntity : Entity
        where TAction : IUpdateActionContext<TEntity>
    {
        private readonly IFieldComponentProvider _provider;

        public EntityFieldComponentUpdateInitiateStep(IFieldComponentProvider provider)
        {
            _provider = provider;
        }

        public override void Execute(TAction actionContext)
        {
            base.Execute(actionContext);
            foreach (var property in typeof(TEntity).GetProperties())
            {
                var componentType = _provider.GetFieldComponent(property.PropertyType);
                if (componentType != null)
                {
                    var instance = ConfigurationProvider.GetService(componentType) as IFieldComponent;
                    if (instance != null)
                    {
                        instance.DisplayName = property.Name;
                        instance.Type = property.PropertyType;
                        instance.SetValue(property.GetValue(actionContext.Entity));
                        actionContext.FieldComponents.Add(instance);
                    }
                }
            }
        }
    }
}
