using System;

namespace Liquid.IoC
{
    public class DefaultImplementationAttribute : Attribute
    {
        private readonly Type _implementation;

        public DefaultImplementationAttribute(Type implementation)
        {
            _implementation = implementation;
        }

        public Type Implementation
        {
            get { return this._implementation; }
        }
    }
}
