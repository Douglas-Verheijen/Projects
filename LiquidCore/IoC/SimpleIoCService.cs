using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Liquid.IoC
{
    public class SimpleIoCService : IIoCService
    {
        private static bool _isConfigured;
        private static SimpleIoCService _instance;
        private readonly ICollection<KeyValuePair<string, Type>> _services;

        protected SimpleIoCService()
        {
            _services = new Collection<KeyValuePair<string, Type>>();
        }

        public static SimpleIoCService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SimpleIoCService();
                    _instance.Initialize();
                }
                return _instance;
            }
        }

        public void Initialize()
        {
            if (!_isConfigured)
            {
                //foreach (var assemblyFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
                //    Assembly.LoadFrom(assemblyFile);

                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Liquid"));
                foreach (var assembly in assemblies)
                    foreach (var type in assembly.GetTypes())
                    {
                        var defaultImplementation = type.GetAttribute<DefaultImplementationAttribute>();
                        if (defaultImplementation != null)
                            if (!Instance._services.Any(x => x.Key == defaultImplementation.Implementation.FullName))
                                Instance._services.Add(new KeyValuePair<string, Type>(defaultImplementation.Implementation.FullName, type));

                        var genericImplementation = type.GetAttribute<GenericImplementationAttribute>();
                        if (genericImplementation != null)
                            Instance._services.Add(new KeyValuePair<string, Type>(genericImplementation.Implementation.FullName, type));

                        if (type.IsClass)
                            if (!Instance._services.Any(x => x.Key == type.FullName))
                                Instance._services.Add(new KeyValuePair<string, Type>(type.FullName, type));
                    }

                _isConfigured = true;
            }
        }

        public ICollection<object> GetServices(Type type)
        {
            if (!type.IsGenericType)
                throw new Exception("IoC: Non Generic type specified.");

            var definition = type.GetGenericTypeDefinition();
            var arguments = type.GetGenericArguments();

            var instances = new Collection<object>();
            var services = Instance._services.Where(x => x.Key == definition.FullName).Select(x => x.Value);
            foreach (var service in services)
            {
                var args = service.GetGenericArguments().Select(x => x.GetGenericParameterConstraints().FirstOrDefault());
                if (arguments.All(x => args.Any(y => x.GUID == y.GUID || y.IsAssignableFrom(x))))
                {
                    var instanceType = service.MakeGenericType(arguments);
                    var instance = CreateClass(instanceType);
                    instances.Add(instance);
                }
            }

            return instances;
        }

        public T GetService<T>()
        {
            Type type = typeof(T);
            T instance = default(T);

            if (type.IsGenericType)
                instance = (T)GetGenericService(type);

            else if (type.IsInterface)
                instance = (T)GetService(type.FullName);

            else if (type.IsClass)
                instance = (T)CreateClass(type);

            return instance;
        }

        private object GetGenericService(Type type)
        {
            var definition = type.GetGenericTypeDefinition();
            var genericType = Instance._services.FirstOrDefault(x => x.Key == definition.FullName).Value;
            var instanceType = genericType.MakeGenericType(type.GetGenericArguments());
            return CreateClass(instanceType);
        }

        public object GetService(string key)
        {
            var type = Instance._services.FirstOrDefault(x => x.Key == key).Value;
            if (type == null)
                throw new Exception(string.Format("IoC: Error occurred trying to resolve type '{0}'. Type not mapped.", key));

            return CreateClass(type);
        }

        private object CreateClass(Type type)
        {
            var ctors = type.GetConstructors();
            var ctor = ctors.FirstOrDefault();
            var parameters = ctor.GetParameters().Select(x => GetService(x.ParameterType.FullName));
            return Activator.CreateInstance(type, BindingFlags.Default, null, parameters.ToArray(), CultureInfo.CurrentCulture);
        }

        public object GetService(Type key)
        {
            return GetService(key.FullName);
        }

        public ICollection<T> GetServices<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<object> GetServices(string key)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<T> GetAllCompatibleInstances<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllCompatibleInstances(Type type)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllCompatibleInstances(Type type, bool filterToMostDerivedTypes)
        {
            throw new NotImplementedException();
        }
    }
}
