using Liquid.IoC.Unity;
using System;
using System.Collections.Generic;

namespace Liquid.IoC
{
    public class ConfigurationProvider
    {
        private static volatile ConfigurationProvider _instance;
        private static object _syncRoot = new object();
        private readonly IIoCService _iocService;

        protected ConfigurationProvider()
        {
            _iocService = new UnityIoCService();
            _iocService.Initialize();
        }

        private static ConfigurationProvider Instance
        {
            get
            {
                if (_instance == null)
                    lock (_syncRoot)
                        _instance = new ConfigurationProvider();

                return _instance;
            }
        }

        public static T GetService<T>()
        {
            return Instance._iocService.GetService<T>();
        }

        public static object GetService(string key)
        {
            return Instance._iocService.GetService(key);
        }

        public static object GetService(Type type)
        {
            return Instance._iocService.GetService(type);
        }

        public static ICollection<object> GetServices(Type type)
        {
            return Instance._iocService.GetServices(type);
        }

        public static IEnumerable<T> GetAllCompatibleInstances<T>()
        {
            return Instance._iocService.GetAllCompatibleInstances<T>();
        }

        public static IEnumerable<object> GetAllCompatibleInstances(Type type)
        {
            return Instance._iocService.GetAllCompatibleInstances(type);
        }

        public static IEnumerable<object> GetAllCompatibleInstances(Type type, bool filterToMostDerivedTypes)
        {
            return Instance._iocService.GetAllCompatibleInstances(type, filterToMostDerivedTypes);
        }
    }
}
