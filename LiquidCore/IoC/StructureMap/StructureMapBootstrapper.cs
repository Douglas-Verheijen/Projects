//using Liquid.Data;
//using Liquid.Services;
//using StructureMap;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Liquid.IoC.StructureMap
//{
//    public static class StructureMapBootstrapper
//    {
//        public static void Bootstrap()
//        {
//            foreach (var assemblyFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
//                Assembly.LoadFrom(assemblyFile);
            
//            ObjectFactory.Initialize(objectFactory =>
//            {
//                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("Liquid"));
//                foreach (var assembly in assemblies)
//                {
//                    foreach (var type in assembly.GetTypes())
//                    {
//                        var defaultImplementation = type.GetAttribute<DefaultImplementationAttribute>();
//                        if (defaultImplementation != null)
//                            objectFactory.For(defaultImplementation.Implementation).Add(type);

//                        var genericImplementation = type.GetAttribute<GenericImplementationAttribute>();
//                        if (genericImplementation != null)
//                            objectFactory.For(genericImplementation.Implementation).Add(type);
//                    }
//                }
//            });
//        }
//    }
//}
