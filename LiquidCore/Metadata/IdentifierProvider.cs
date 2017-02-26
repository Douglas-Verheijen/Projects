using Liquid.Data;
using Liquid.IoC;
using System;
using System.Linq;

namespace Liquid.Metadata
{
    public interface IIdentifierProvider
    {
        object GetIdentifier(IPersistentObject obj);
        void SetIdentifier(IPersistentObject obj, object value);
    }

    [DefaultImplementation(typeof(IIdentifierProvider))]
    class IdentifierProvider : IIdentifierProvider
    {
        public object GetIdentifier(IPersistentObject obj)
        {
            Type type = obj.GetType();
            var property = type.GetProperties().FirstOrDefault(x => x.HasAttribute<IdentifierAttribute>());
            if (property != null)
                return property.GetValue(obj);

            return obj.ToString();
        }

        public void SetIdentifier(IPersistentObject obj, object value)
        {
            Type type = obj.GetType();
            var property = type.GetProperties().FirstOrDefault(x => x.HasAttribute<IdentifierAttribute>());
            if (property != null)
                property.SetValue(obj, value);
        }
    }
}
