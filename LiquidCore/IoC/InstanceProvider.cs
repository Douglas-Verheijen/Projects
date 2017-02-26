using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.IoC
{
    public interface IInstanceProvider<T>
    {
        T GetInstance();
    }

    [DefaultImplementation(typeof(IInstanceProvider<>))]
    class InstanceProvider<T> : IInstanceProvider<T>
    {
        public T GetInstance()
        {
            return ConfigurationProvider.GetService<T>();
        }
    }
}
