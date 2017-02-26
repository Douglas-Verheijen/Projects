//using Liquid.Actions;
//using Liquid.Actions.Process;
//using Liquid.Domain.Security;
//using StructureMap;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Liquid.IoC.StructureMap
//{
//    class StructureMapIoCService : IIoCService
//    {
//        public void Initialize()
//        {
//            StructureMapBootstrapper.Bootstrap();
//        }

//        public T GetService<T>()
//        {
//            return ObjectFactory.GetInstance<T>();
//        }

//        public object GetService(string key)
//        {
//            var type = Type.GetType(key);
//            return GetService(type);
//        }

//        public object GetService(Type type)
//        {
//            return ObjectFactory.GetInstance(type);
//        }

//        public ICollection<T> GetServices<T>()
//        {
//            var instances = new Collection<T>();
//            foreach (T instance in ObjectFactory.GetAllInstances<T>())
//                instances.Add(instance);
//            return instances;
//        }

//        public ICollection<object> GetServices(string key)
//        {
//            var type = Type.GetType(key);
//            return GetServices(type);
//        }

//        public ICollection<object> GetServices(Type type)
//        {
//            var obj = ObjectFactory.WhatDoIHave();

//            var instances = new Collection<object>();
//            foreach (var instance in ObjectFactory.GetAllInstances(type.GetGenericTypeDefinition()))
//                instances.Add(instance);
//            return instances;
//        }


//        public IEnumerable<T> GetAllCompatibleInstances<T>()
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<object> GetAllCompatibleInstances(Type type)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<object> GetAllCompatibleInstances(Type type, bool filterToMostDerivedTypes)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
