using Liquid.Components;
using Liquid.Domain;
using Liquid.IoC;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace Liquid.Actions
{
    public interface IFormActionContext
    {
        ICollection<string> Errors { get; set; }
        ICollection<IFieldComponent> FieldComponents { get; set; }
        Entity GetEntity();
        string GetEntityType();
        bool HasErrors();
    }

    public interface IFormActionContext<TEntity> : IFormActionContext
        where TEntity : Entity
    {
        TEntity Entity { get; set; }
    }

    [GenericImplementation(typeof(IFormActionContext<>))]
    class FormActionContext<TEntity> : IFormActionContext<TEntity> 
        where TEntity : Entity
    {
        public FormActionContext()
        {
            Errors = new List<string>();
            FieldComponents = new List<IFieldComponent>();
        }

        public TEntity Entity { get; set; }

        public ICollection<string> Errors { get; set; }

        public ICollection<IFieldComponent> FieldComponents { get; set; }
        
        public bool HasErrors()
        {
            return Errors.Any();
        }

        public Entity GetEntity()
        {
            return Entity;
        }

        public string GetEntityType()
        {
            return PluralizationService
                .CreateService(CultureInfo.CurrentCulture)
                .Pluralize(typeof(TEntity).Name);
        }
    }
}
