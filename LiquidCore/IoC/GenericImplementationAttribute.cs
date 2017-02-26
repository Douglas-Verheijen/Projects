using System;

namespace Liquid.IoC
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GenericImplementationAttribute : Attribute
    {
        private readonly Type _implementation;

        public GenericImplementationAttribute(Type implementation)
        {
            _implementation = implementation;
        }

        public Type Implementation
        {
            get { return this._implementation; }
        }
    }
}
