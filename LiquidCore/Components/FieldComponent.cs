using System;
using System.Reflection;

namespace Liquid.Components
{
    public interface IFieldComponent
    {
        string DisplayName { get; set; }
        string ComponentName { get; }
        Type Type { get; set; }

        object GetValue();
        void SetValue(object value);
    }

    public abstract class FieldComponent<T> : IFieldComponent
    {
        public string DisplayName { get; set; }

        public T Value { get; set; }

        public Type Type { get; set; }

        public string ComponentName
        {
            get { return GetType().Name; }
        }

        public object GetValue()
        {
            return Value;
        }

        public void SetValue(object value)
        {
            if (value is T)
                Value = (T)value;
        }
    }
}