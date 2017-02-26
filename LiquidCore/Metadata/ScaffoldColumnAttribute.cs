using System;

namespace Liquid.Metadata
{
    public class ScaffoldColumnAttribute : Attribute
    {
        public ScaffoldColumnAttribute(bool value)
        {
            Value = value;
        }

        public virtual bool Value { get; set; }
    }
}
