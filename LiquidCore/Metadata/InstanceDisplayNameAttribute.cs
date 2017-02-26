using System;

namespace Liquid.Metadata
{
    public class InstanceDisplayNameAttribute : Attribute
    {
        private readonly string _displayName;

        public InstanceDisplayNameAttribute(string displayName)
        {
            _displayName = displayName;
        }

        public string DisplayName
        {
            get { return _displayName; }
        }
    }
}
