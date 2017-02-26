using NHibernate.Mapping.ByCode;
using System;

namespace Liquid.Data.NHibernate
{
    public class NHibernateModelMapper : ModelMapper
    {
        public NHibernateModelMapper()
            : base()
        {
            BeforeMapClass += NHibernateModelMapper_BeforeMapClass;
            BeforeMapProperty += NHibernateModelMapper_BeforeMapProperty;
        }

        private void NHibernateModelMapper_BeforeMapProperty(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
        }

        private void NHibernateModelMapper_BeforeMapClass(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {
        }
    }
}
