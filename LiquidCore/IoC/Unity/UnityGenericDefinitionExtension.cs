using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Liquid.IoC.Unity
{
    class GenericTypeDefinitionMappedBuilderStrategy : BuilderStrategy
    {
        private readonly Dictionary<Type, IList<Type>> _registeredGenericTypeDefinitions;

        public GenericTypeDefinitionMappedBuilderStrategy(Dictionary<Type, IList<Type>> registeredGenericTypeDefinitions)
        {
            this._registeredGenericTypeDefinitions = registeredGenericTypeDefinitions;
        }

        public IEnumerable<Type> GetMappedTypes(Type genericType, bool filterToMostDerivedTypes)
        {
            if (genericType.IsInterface && genericType.IsGenericType)
            {
                var mappedTypes = this.GetMappedResolveType(genericType);
                return mappedTypes.Where(mostDerivedType => mappedTypes.Where(x => x != mostDerivedType).All(x => !mostDerivedType.IsAssignableFrom(x)));
            }

            return Enumerable.Empty<Type>();
        }

        private IEnumerable<Type> GetMappedResolveType(Type type)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();

            IList<Type> mappedTypeDefinitions;
            if (this._registeredGenericTypeDefinitions.TryGetValue(genericTypeDefinition, out mappedTypeDefinitions))
            {
                var mappedDefinitionTypes = mappedTypeDefinitions
                    .Select(mappedType => GetGenericTypeDefinitionMap(type, mappedType));

                if (mappedDefinitionTypes != null)
                {
                    var mappedTypes = new List<Type>();

                    foreach (var mappedDefinitionType in mappedDefinitionTypes)
                    {
                        var typeArguments = mappedDefinitionType.ArgumentMaps.OrderBy(argumentMap => argumentMap.MapToArgument.GenericParameterPosition)
                            .Select(argumentMap => argumentMap.MapFromType)
                            .ToArray();

                        if (mappedDefinitionType.MappedType.IsGenericTypeDefinition)
                        {
                            Type mappedType; 
                            try
                            {
                                mappedType = mappedDefinitionType.MappedType.MakeGenericType(typeArguments);
                                if (!type.IsCompatibleWith(mappedType))
                                    mappedType = null;
                            }
                            catch (Exception)
                            {
                                mappedType = null;
                            }

                            if (mappedType != null)
                                yield return mappedType;
                        }
                        else
                            yield return mappedDefinitionType.MappedType;
                    }
                }
            }
        }

        private static GenericTypeDefinitionMap GetGenericTypeDefinitionMap(Type resolveType, Type mappedTypeDefinition)
        {
            var mappedTypeDefinitionGenericArguments = mappedTypeDefinition.GetGenericArguments();
            var genericTypeDefinitionMap = new GenericTypeDefinitionMap()
            {
                ResolveType = resolveType,
                MappedType = mappedTypeDefinition,
                ArgumentMaps = mappedTypeDefinitionGenericArguments.Select(mapToArg => new GenericTypeDefinitionArgumentMap() { MapToArgument = mapToArg }).ToArray()
            };

            InitializeGenericTypeDefinitionArgumentMap(resolveType, mappedTypeDefinition, genericTypeDefinitionMap);

            var mappedInterfaceGenericDefinition = mappedTypeDefinition.GetInterfaces().SingleOrDefault(interfaceType =>
                interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == resolveType.GetGenericTypeDefinition());

            var chainedGenericArgumentTypes = new HashSet<Type>(mappedTypeDefinition.GetGenericArguments());
            genericTypeDefinitionMap.Distances = MeasureArgumentDistancesAndMapArguments(genericTypeDefinitionMap, resolveType, mappedInterfaceGenericDefinition, chainedGenericArgumentTypes);

            return genericTypeDefinitionMap;
        }

        private static int[] MeasureArgumentDistancesAndMapArguments(GenericTypeDefinitionMap genericTypeDefinitionMap, Type resolveType, Type mappedType, ICollection<Type> chainedGenericArgumentTypes)
        {
            var nearestResolveType = GetNearestResolveTypeWithSameArguments(resolveType, mappedType);

            if (!mappedType.IsCompatibleWith(nearestResolveType))
                return new int[] { -1 };

            var mappedTypeGenericArguments = mappedType.GetGenericArguments();
            HashSet<Type> updatedChainedGenericArgumentTypes = new HashSet<Type>(chainedGenericArgumentTypes.Union(mappedTypeGenericArguments));

            var nearestResolveTypeGenericArguments = nearestResolveType.GetGenericArguments();
            return nearestResolveTypeGenericArguments
                .Zip(mappedType.GetGenericArguments(),
                    (resolveTypeArg, mappedTypeArg) => new { ResolveTypeArg = resolveTypeArg, MappedTypeArg = mappedTypeArg })
                .SelectMany(argPair => MeasureSingleArgumentDistancesAndMapArguments(genericTypeDefinitionMap, argPair.ResolveTypeArg, argPair.MappedTypeArg, updatedChainedGenericArgumentTypes))
                .ToArray();
        }

        private static int[] MeasureSingleArgumentDistancesAndMapArguments(GenericTypeDefinitionMap genericTypeDefinitionMap, Type resolveArg, Type typeDefArg, ICollection<Type> chainedGenericArgumentTypes)
        {
            var mappedArg = genericTypeDefinitionMap.ArgumentMaps.FirstOrDefault(argMap => String.Equals(argMap.MapToArgument.Name, typeDefArg.Name));

            if (mappedArg != null)
            {
                //THere is already a constraint on the parameter
                if (mappedArg.MapFromType != null)
                {
                    //If constraint types within nested generic arguments of generic types don't match, then this is an invalid mapping
                    if (mappedArg.MapFromType != resolveArg)
                    {
                        return new int[] { -1 };
                    }
                }
                else
                    mappedArg.MapFromType = resolveArg;
            }

            var typedDefArgConstraints = new Type[] { typeDefArg };

            if (typeDefArg.IsGenericParameter)
            {
                if (typeDefArg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint) &&
                    resolveArg.IsValueType)
                    return new int[] { -1 };

                if (typeDefArg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint) &&
                    !resolveArg.IsValueType)
                    return new int[] { -1 };

                typedDefArgConstraints = typeDefArg.GetGenericParameterConstraints();
            }

            if (typedDefArgConstraints.Length == 0)
                typedDefArgConstraints = new Type[] { typeof(object) };

            // if this is the last type to be resolved then confirm it is all ok
            // in particular, we are looking for cases where there are lots of interactions between the generic arguments where it needs to
            // actually consider the specific types found not just whether the generic types are compatible
            if (genericTypeDefinitionMap.MappedType.IsGenericType &&
                genericTypeDefinitionMap.ArgumentMaps.All(x => x.MapFromType != null))
            {
                var typeArguments = genericTypeDefinitionMap.ArgumentMaps.OrderBy(
                        argumentMap => argumentMap.MapToArgument.GenericParameterPosition)
                                            .Select(argumentMap => argumentMap.MapFromType)
                                            .ToArray();

                var isValid = genericTypeDefinitionMap.MappedType.IsValidWithGenericArguments(typeArguments);

                if (!isValid)
                    return new int[] { -1 };
            }

            var constraintDistances = typedDefArgConstraints.Select(typedDefArgConstraint => MeasureArgumentConstraintDistance(genericTypeDefinitionMap, resolveArg, typedDefArgConstraint, chainedGenericArgumentTypes, typeDefArg)).ToArray();

            if (!constraintDistances.Any())
                return null;

            return constraintDistances.SelectMany(x => x).ToArray();
        }

        internal static Type GetNearestResolveTypeWithSameArguments(Type type, Type typeWithArgumentsToExtract)
        {
            if (type.GUID == typeWithArgumentsToExtract.GUID)
                return type;
            else if (typeWithArgumentsToExtract.IsInterface)
            {
                var resolveInterface = type.GetInterfaces().FirstOrDefault(x => x.GUID == typeWithArgumentsToExtract.GUID);
                if (resolveInterface != null)
                    return resolveInterface;
            }

            if (type.BaseType != null && typeWithArgumentsToExtract.IsCompatibleWith(type.BaseType))
                return GetNearestResolveTypeWithSameArguments(type.BaseType, typeWithArgumentsToExtract);
            else
                return type;
        }

        private static void InitializeGenericTypeDefinitionArgumentMap(Type resolveType, Type mappedTypeDefinition,
                                                                       GenericTypeDefinitionMap genericTypeDefinitionMap)
        {
            if (!resolveType.IsGenericType)
            {
                return;
            }
            var genericTypeDefinition = resolveType.GetGenericTypeDefinition();

            Type[] mappedGenericDefinitionArguments;
            if (!mappedTypeDefinition.TryGetGenericArgumentsFromSubclassOfGenericDefinition(genericTypeDefinition, out mappedGenericDefinitionArguments)) return;

            Type[] resolvedGenericDefinitionArguments;
            if (!resolveType.TryGetGenericArgumentsFromSubclassOfGenericDefinition(genericTypeDefinition, out resolvedGenericDefinitionArguments)) return;

            for (var index = 0; index < mappedGenericDefinitionArguments.Length; index++)
            {
                var argumentToMap =
                    genericTypeDefinitionMap.ArgumentMaps.FirstOrDefault(
                        x => x.MapToArgument.Name == mappedGenericDefinitionArguments[index].Name);
                if (argumentToMap != null)
                {
                    argumentToMap.MapFromType = resolvedGenericDefinitionArguments[index];
                }
            }
        }

        private static int[] MeasureArgumentConstraintDistance(GenericTypeDefinitionMap genericTypeDefinitionMap, Type resolveArg, Type typedDefArgConstraint, ICollection<Type> chainedGenericArgumentTypes, Type typeDefArg)
        {
            if (!typedDefArgConstraint.IsCompatibleWith(resolveArg))
                return new int[] { -1 };

            var compareType = resolveArg;
            int count = -1;

            var chainedConstraintList = chainedGenericArgumentTypes.Where(x => x.Name == typeDefArg.Name)
                .SelectMany(constraint => constraint.IsGenericParameter ? constraint.GetGenericParameterConstraints() : new Type[] { constraint })
                .Union(new Type[] { typedDefArgConstraint })
                .ToList();

            if (!compareType.IsInterface)
            {
                while (compareType != null && chainedConstraintList.All(constraint => constraint.IsCompatibleWith(compareType)))
                {
                    count++;
                    compareType = compareType.BaseType;
                }
            }
            else
            {
                if (chainedConstraintList.All(constraint => constraint.IsCompatibleWith(compareType)))
                {
                    if (chainedConstraintList.All(x => x == typeof(Object)))
                        count = 1;
                    else
                        count = 0;

                    var interfaces = compareType.GetInterfaces()
                        .Where(x => chainedConstraintList.All(constraint => constraint.IsCompatibleWith(x)))
                        .ToList();

                    //top level interface within the list
                    var topLevelInterfaces = interfaces.Where(x => !interfaces.Any(y => x != y && x.IsAssignableFrom(y))).ToList();

                    while (interfaces.Count > 0)
                    {
                        count++;
                        interfaces = interfaces.Where(x => !topLevelInterfaces.Any(y => y == x)).ToList();
                        topLevelInterfaces = interfaces.Where(x => !interfaces.Any(y => x != y && x.IsAssignableFrom(y))).ToList();
                    }
                }
            }

            if (typedDefArgConstraint.IsGenericType)
            {
                //if argument is generic, we'll need to measure distance of arguments of the generic argument (nested),
                //as such build up the current constaints and nest them

                var argumentDistances = MeasureArgumentDistancesAndMapArguments(
                    genericTypeDefinitionMap,
                    resolveArg,
                    // workaround, null FullName means it's a generic type constrained with arguments from a higher level type that it is a nested argument in
                    (typedDefArgConstraint.FullName == null ? typedDefArgConstraint : typedDefArgConstraint.GetGenericTypeDefinition()),
                    chainedGenericArgumentTypes);

                return new int[] { count }.Concat(argumentDistances).ToArray();
            }
            else
                return new int[] { count };
        }

        private class DistanceComparer : IComparer<int[]>
        {
            public int Compare(int[] x, int[] y)
            {
                foreach (var intPair in x.Zip(y, (x1, y1) => new { x1 = x1, y1 = y1 }))
                {
                    if (intPair.x1 > intPair.y1)
                        return 1;
                    else if (intPair.x1 < intPair.y1)
                        return -1;
                }

                return 0;
            }
        }

        private class GenericTypeDefinitionMap
        {
            public Type ResolveType { get; set; }
            public Type MappedType { get; set; }
            public int[] Distances { get; set; }
            public GenericTypeDefinitionArgumentMap[] ArgumentMaps { get; set; }
        }

        private class GenericTypeDefinitionArgumentMap
        {
            public Type MapToArgument { get; set; }
            public Type MapFromType { get; set; }
        }

        private static int GetOverallArgumentDistance(int[] distances)
        {
            if (distances.Any(x => x > 9))
                throw new Exception("Argument inheritance distance greater than 9 detected");

            return int.Parse(String.Join(String.Empty, distances.Select(x => x.ToString(CultureInfo.InvariantCulture))), CultureInfo.InvariantCulture);
        }
    }

    public class UnityGenericDefinitionExtension : UnityContainerExtension
    {
        private readonly Dictionary<Type, IList<Type>> _registeredGenericTypeDefinitions = new Dictionary<Type, IList<Type>>();
        private GenericTypeDefinitionMappedBuilderStrategy _genericDefinitionStrategy;

        protected override void Initialize()
        {
            _genericDefinitionStrategy = new GenericTypeDefinitionMappedBuilderStrategy(this._registeredGenericTypeDefinitions);
            Context.Strategies.Add(_genericDefinitionStrategy, Microsoft.Practices.Unity.ObjectBuilder.UnityBuildStage.Setup);
        }

        internal void RegisterGenericTypeDefinition(Type typeFrom, Type typeTo)
        {
            if (typeFrom != null &&
                typeTo != null &&
                typeFrom.IsInterface &&
                typeFrom.IsGenericTypeDefinition)
            {
                IList<Type> mappedTypes;

                if (!this._registeredGenericTypeDefinitions.TryGetValue(typeFrom, out mappedTypes))
                    mappedTypes = this._registeredGenericTypeDefinitions[typeFrom] = new List<Type>();
                mappedTypes.Insert(0, typeTo);
            }
        }

        public IEnumerable<T> ResolveAll<T>(IUnityContainer instanceContainer, bool filterToMostDerivedTypes = true)
        {
            var list = _genericDefinitionStrategy.GetMappedTypes(typeof(T), filterToMostDerivedTypes);
            return list.Select(x => (T)instanceContainer.Resolve(x));
        }

        public IEnumerable<object> ResolveAll(IUnityContainer instanceContainer, Type genericType, bool filterToMostDerivedTypes = true)
        {
            var list = _genericDefinitionStrategy.GetMappedTypes(genericType, filterToMostDerivedTypes);
            return list.Select(x => instanceContainer.Resolve(x));
        }
    }
}
