using Liquid.Data;
using Liquid.IoC;
using System;
using System.Reflection;

namespace Liquid.Metadata
{
    public interface IInstanceDisplayNameProvider
    {
        string GetDisplayName(IPersistentObject obj);
    }

    [DefaultImplementation(typeof(IInstanceDisplayNameProvider))]
    class InstanceDisplayNameProvider : IInstanceDisplayNameProvider
    {
        public string GetDisplayName(IPersistentObject obj)
        {
            Type type = obj.GetType();
            InstanceDisplayNameAttribute attribute = type.GetAttribute<InstanceDisplayNameAttribute>();
            if (attribute != null)
            {
                string displayName = attribute.DisplayName;
                if (displayName.Contains("{{") && displayName.Contains("}}"))
                {
                    string propertyName = displayName.Replace("{{", string.Empty).Replace("}}", string.Empty);
                    PropertyInfo property = type.GetProperty(propertyName);
                    if (property != null)
                    {
                        var value = property.GetValue(obj, null);
                        return value != null ? value.ToString() : null;
                    }
                }

                return displayName;
            }

            return obj.Id.ToString();
        }
    }
}
