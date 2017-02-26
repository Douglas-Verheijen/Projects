using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Liquid.IoC.Unity
{
    class UnityIoCService : IIoCService
    {
        private readonly IUnityContainer _container;

        public UnityIoCService()
        {
            _container = new UnityContainer();
            _container.AddNewExtension<UnityGenericDefinitionExtension>();
        }

        public void Initialize()
        {
            foreach (var assemblyFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
                Assembly.LoadFrom(assemblyFile);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Liquid"));
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var defaultImplementation = type.GetAttribute<DefaultImplementationAttribute>();
                    if (defaultImplementation != null)
                        _container.RegisterType(defaultImplementation.Implementation, type);

                    var genericImplementation = type.GetAttribute<GenericImplementationAttribute>();
                    if (genericImplementation != null)
                        _container.Configure<UnityGenericDefinitionExtension>()
                            .RegisterGenericTypeDefinition(genericImplementation.Implementation, type);
                }
            }
        }

        public T GetService<T>()
        {
            var type = typeof(T);
            if (type.IsGenericType)
            {
                var instances = GetAllCompatibleInstances(type, true);
                return (T)instances.FirstOrDefault();
            }

            return _container.Resolve<T>();
        }

        public object GetService(string key)
        {
            var type = Type.GetType(key);
            return GetService(type);
        }

        public object GetService(Type type)
        {
            if (type.IsGenericTypeDefinition)
            {
                var instances = GetAllCompatibleInstances(type, true);
                return instances.FirstOrDefault();
            }

            return _container.Resolve(type);
        }

        public ICollection<T> GetServices<T>()
        {
            return _container.ResolveAll<T>().ToList();
        }

        public ICollection<object> GetServices(string key)
        {
            var type = Type.GetType(key);
            return GetServices(type);
        }

        public ICollection<object> GetServices(Type type)
        {
            return _container.ResolveAll(type).ToList();
        }

        public IEnumerable<T> GetAllCompatibleInstances<T>()
        {
            foreach (T instance in GetAllCompatibleInstances(typeof(T), true))
                yield return instance;
        }

        public IEnumerable<object> GetAllCompatibleInstances(Type type)
        {
            return GetAllCompatibleInstances(type, true);
        }

        public IEnumerable<object> GetAllCompatibleInstances(Type type, bool filterToMostDerivedTypes)
        {
            return _container.Configure<UnityGenericDefinitionExtension>().ResolveAll(_container, type, filterToMostDerivedTypes);
        }
    }
}
