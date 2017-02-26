using Liquid.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Components
{
    public interface IFieldComponentProvider
    {
        Type GetFieldComponent<T>();
        Type GetFieldComponent(Type primitive);
    }

    [DefaultImplementation(typeof(IFieldComponentProvider))]
    class FieldComponentProvider : IFieldComponentProvider
    {
        private IEnumerable<Type> _componentTypes;

        public FieldComponentProvider()
        {
            _componentTypes = Init();
        }

        public IEnumerable<Type> Init()
        {
            var componentType = typeof(IEditComponent);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Liquid")))
                foreach (var type in assembly.GetTypes().Where(x => !x.IsAbstract && componentType.IsAssignableFrom(x)))
                    yield return type;
        }

        public Type GetFieldComponent<T>()
        {
            return GetFieldComponent(typeof(T));
        }

        public Type GetFieldComponent(Type primitive)
        {
            return _componentTypes.FirstOrDefault(x => x.BaseType.IsGenericType 
                && x.BaseType.GenericTypeArguments.Any(y => y.IsAssignableFrom(primitive)));
        }
    }
}
