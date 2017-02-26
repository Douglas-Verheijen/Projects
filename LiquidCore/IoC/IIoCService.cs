using System;
using System.Collections.Generic;

namespace Liquid.IoC
{
    public interface IIoCService
    {
        void Initialize();
        T GetService<T>();
        object GetService(string key);
        object GetService(Type key);
        ICollection<T> GetServices<T>();
        ICollection<object> GetServices(string key);
        ICollection<object> GetServices(Type type);
        IEnumerable<T> GetAllCompatibleInstances<T>();
        IEnumerable<object> GetAllCompatibleInstances(Type type);
        IEnumerable<object> GetAllCompatibleInstances(Type type, bool filterToMostDerivedTypes);
    }
}
