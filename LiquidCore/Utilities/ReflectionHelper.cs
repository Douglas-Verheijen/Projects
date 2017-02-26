using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Liquid.Utilities
{
    public static class ReflectionHelper
    {
        //  Not very practical, but may come in handy.
        public static object GetPropertyValue<T>(T obj, Expression<Func<T, object>> expression)
        {
            var property = typeof(T).GetProperty(GetPropertyName(expression));
            return property.GetValue(obj, null);
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }
    }
}
