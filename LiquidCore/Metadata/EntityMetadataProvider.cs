using Liquid.Domain;
using Liquid.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Liquid.Metadata
{
    public interface IEntityMetadataProvider
    {
        IEnumerable<Type> GetEntityTypes();
    }

    [DefaultImplementation(typeof(IEntityMetadataProvider))]
    class EntityMetadataProvider : IEntityMetadataProvider
    {
        public IEnumerable<Type> GetEntityTypes()
        {
            var directoryName = AppDomain.CurrentDomain.BaseDirectory;
            if (!(directoryName.EndsWith("\\bin") || directoryName.EndsWith("\\bin\\Debug")))
                directoryName += "\\bin";

            foreach (var assemblyFile in Directory.GetFiles(directoryName, "Liquid*.dll"))
            {
                var assembly = Assembly.LoadFrom(assemblyFile);
                var types = assembly.GetTypes().Where(x => typeof(Entity).IsAssignableFrom(x));
                foreach (var type in types)
                    yield return type;
            }
        }
    }
}
