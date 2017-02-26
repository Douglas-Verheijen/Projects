using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Liquid
{
    public static class TypeExtensions
    {
        public static IEnumerable<TAttribute> Where<TAttribute>(this AttributeCollection attributes, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
        {
            return predicate == null
                ? attributes.OfType<TAttribute>()
                : attributes.OfType<TAttribute>().Where(predicate);
        }

        public static TAttribute FirstOrDefault<TAttribute>(this AttributeCollection attributes, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
        {
            return attributes.Where(predicate).FirstOrDefault();
        }

        public static T GetAttribute<T>(this Type type, Func<T, bool> predicate = null)
            where T : Attribute
        {
            return TypeDescriptor.GetAttributes(type).FirstOrDefault(predicate);
        }

        public static bool HasAttribute<TAttribute>(this Type type, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
        {
            return type.GetAttribute(predicate) != null;
        }

        public static bool HasAttribute<TAttribute>(this MemberInfo member)
            where TAttribute : Attribute
        {
            Contract.Requires(member != null);
            return member.GetCustomAttributes(typeof(TAttribute), true).Any();
        }

        public static IEnumerable<Type> GetGenericInterfaces(this Type type, Type openGenericInterfaceRestriction = null)
        {
            Contract.Requires(type != null);
            return openGenericInterfaceRestriction == null
                ? type.GetInterfaces().Where(i => i.IsGenericType)
                : type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericInterfaceRestriction);
        }

        public static bool IsSubclassOfGenericDefinition(this Type toCheck, Type genericTypeDefinition)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (genericTypeDefinition == cur)
                {
                    return true;
                }
                toCheck = (toCheck.IsInterface || genericTypeDefinition.IsInterface) ? toCheck.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericTypeDefinition) : toCheck.BaseType;
            }
            return false;
        }

        public static bool TryGetGenericArgumentsFromSubclassOfGenericDefinition(this Type toCheck, Type genericTypeDefinition, out Type[] genericArguments)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (genericTypeDefinition == cur)
                {
                    genericArguments = toCheck.GetGenericArguments();
                    return true;
                }
                toCheck = (toCheck.IsInterface || genericTypeDefinition.IsInterface) ? toCheck.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericTypeDefinition) : toCheck.BaseType;
            }
            genericArguments = null;
            return false;
        }

        public static bool IsCompatibleWith(this Type type, Type compareTo)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (compareTo == null) throw new ArgumentNullException("compareTo");

            if (type.IsClass && compareTo.IsInterface)
                return type.IsAssignableFrom(compareTo);
            else if (type.IsGenericTypeDefinition)
            {
                return compareTo.IsSubclassOfGenericDefinition(type);
            }
            else if (type.ContainsGenericParameters ||
                compareTo.ContainsGenericParameters)
            {
                if (type.IsGenericType)
                {
                    Type[] compatibleTypeArgs = null;
                    var typeDefinition = type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition();

                    if (!compareTo.IsGenericType)
                        compareTo.TryGetGenericArgumentsFromSubclassOfGenericDefinition(typeDefinition, out compatibleTypeArgs);
                    else
                    {
                        var compareToDefinition = compareTo.IsGenericTypeDefinition ? compareTo : compareTo.GetGenericTypeDefinition();

                        if (typeDefinition == compareToDefinition)
                            compatibleTypeArgs = compareTo.GetGenericArguments();
                        else if (type.IsInterface)
                        {
                            var compatibleMatchingType = compareTo.GetInterfaces()
                                    .FirstOrDefault(compareToInterface => compareToInterface.IsGenericType && compareToInterface.GetGenericTypeDefinition() == type.GetGenericTypeDefinition());

                            if (compatibleMatchingType != null)
                                compatibleTypeArgs = compatibleMatchingType.GetGenericArguments();
                        }
                        else
                            compareTo.TryGetGenericArgumentsFromSubclassOfGenericDefinition(typeDefinition, out compatibleTypeArgs);
                    }

                    if (compatibleTypeArgs == null)
                        return false;
                    else
                    {
                        var typeArgs = type.GetGenericArguments();

                        if (typeArgs.Length != compatibleTypeArgs.Length)
                            throw new Exception("Argument Lengths do not match, invalid logic");

                        var matchingArgTypes = typeArgs
                            //.Select((x, y) => new { arg = x, row=y})
                            .Zip(compatibleTypeArgs, (x, y) => new { typeArg = x, compatibleTypeArg = y })
                            .ToList();


                        return matchingArgTypes.All(x =>
                        {
                            //if both args are parameter types, then they are compatible as we already know one contains the other definition
                            if (x.compatibleTypeArg.IsGenericParameter && x.typeArg.IsGenericParameter)
                                return true;
                            else
                            {
                                if (x.compatibleTypeArg.IsGenericParameter)
                                {
                                    var compatibleConstraints = x.compatibleTypeArg.GetGenericParameterConstraints();
                                    return compatibleConstraints
                                        .Where(constraint => constraint != type) //avoiding self-referencing types
                                        .All(constraint => constraint.IsCompatibleWith(x.typeArg));
                                }
                                else if (x.typeArg.IsGenericParameter)
                                {
                                    var typeConstraints = x.typeArg.GetGenericParameterConstraints();
                                    return typeConstraints
                                        .Where(constraint => constraint != type) //avoiding self-referencing types
                                        .All(constraint => constraint.IsCompatibleWith(x.compatibleTypeArg));
                                }
                                else
                                    return x.typeArg.IsCompatibleWith(x.compatibleTypeArg);
                            }
                        });
                    }
                }
                else if (type.IsGenericParameter)
                {
                    return type.GetGenericParameterConstraints().All(x =>
                        x.IsCompatibleWith(compareTo));
                }
                else
                    return type.IsAssignableFrom(compareTo);
            }
            else
                return type.IsAssignableFrom(compareTo);
        }

        public static bool IsValidWithGenericArguments(this Type genericType, params Type[] typeArguments)
        {
            if (genericType == null)
                throw new ArgumentNullException("genericType");

            if (typeArguments == null)
                throw new ArgumentNullException("typeArguments");

            Contract.Assume(genericType.IsGenericType);
            Contract.Assume(genericType.GetGenericArguments().Length == typeArguments.Length);

            return IsValidWithGenericArguments(genericType, typeArguments, null, null);
        }

        private static bool IsValidWithGenericArguments(Type genericType, Type[] typeArguments, HashSet<Type> processedConstraints, Dictionary<string, Type> topLevelMappedArgumentParameters)
        {
            if (processedConstraints == null)
                processedConstraints = new HashSet<Type>();

            var genericDefinitionArguments = genericType.GetGenericArguments();
            Contract.Assume(genericDefinitionArguments != null);
            var genericDefinitionParameters = genericDefinitionArguments.Where(x => x.IsGenericParameter).ToArray();

            if (topLevelMappedArgumentParameters == null)
            {
                topLevelMappedArgumentParameters = new Dictionary<string, Type>();

                for (int i = 0; i < genericDefinitionParameters.Length; i++)
                {
                    topLevelMappedArgumentParameters.Add(genericDefinitionParameters[i].Name, typeArguments[i]);
                }
            }

            bool isValid = true;


            foreach (var genericArgument in genericDefinitionParameters)
            {
                var genericParameterAttributes = genericArgument.GenericParameterAttributes;

                var genericParameterSpecialConstraints = genericParameterAttributes &
                    GenericParameterAttributes.SpecialConstraintMask;

                if ((genericParameterSpecialConstraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
                {
                    isValid = topLevelMappedArgumentParameters[genericArgument.Name].IsValueType && !topLevelMappedArgumentParameters[genericArgument.Name].IsNullable();
                }
                else
                {
                    var isValueType = topLevelMappedArgumentParameters[genericArgument.Name].IsValueType;

                    if ((genericParameterSpecialConstraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
                    {
                        isValid = !isValueType;
                    }

                    if (isValid && !isValueType && (genericParameterSpecialConstraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
                    {
                        isValid = topLevelMappedArgumentParameters[genericArgument.Name].GetConstructor(new Type[] { }) != null;
                    }
                }

                if (isValid)
                {
                    Contract.Assume(genericArgument.IsGenericParameter);

                    var constraints = genericArgument.GetGenericParameterConstraints();

                    foreach (var constraint in constraints)
                    {
                        Contract.Assume(constraint != null);


                        if (constraint.IsGenericType && constraint.ContainsGenericParameters)
                        {
                            var constraintGenericArgumentsParameters = constraint.GetGenericArguments()
                                .Where(x => x.IsGenericParameter)
                                .Select(x => topLevelMappedArgumentParameters[x.Name])
                                    .ToArray();

                            //Only process constraint if the type wasn't yet processed (as it could be recursively point back to itself
                            if (!processedConstraints.Contains(constraint))
                            {
                                //must add it to the list before calling the nested constrains otherwise for recursive constraint, it would end up being infinite loop
                                processedConstraints.Add(constraint);
                                isValid = IsValidWithGenericArguments(constraint, constraintGenericArgumentsParameters, processedConstraints, topLevelMappedArgumentParameters);
                            }

                            if (isValid)
                            {
                                var constraintArgs = constraint.GetGenericArguments();
                                Contract.Assume(constraintArgs != null);
                                var constraintGenericArguments = constraintArgs.Select(x =>
                                {
                                    Type returnConstraintType = null;

                                    if (x.IsGenericParameter)
                                    {
                                        returnConstraintType = topLevelMappedArgumentParameters[x.Name];
                                    }
                                    else if (x.IsGenericType && x.ContainsGenericParameters)
                                    {
                                        var constraintArguments =
                                            x.GetGenericArguments()
                                                .Where(y => y.IsGenericParameter)
                                                .Select(y => topLevelMappedArgumentParameters[y.Name])
                                                .ToArray();

                                        isValid = isValid && IsValidWithGenericArguments(x, constraintArguments, processedConstraints, topLevelMappedArgumentParameters);

                                        if (isValid)
                                        {
                                            var nestedConstraintGenericDefinition = x.GetGenericTypeDefinition();
                                            Contract.Assume(nestedConstraintGenericDefinition.IsGenericTypeDefinition);
                                            Contract.Assume(
                                                nestedConstraintGenericDefinition.GetGenericArguments().Length
                                                == constraintArguments.Length);
                                            returnConstraintType = nestedConstraintGenericDefinition.MakeGenericType(constraintArguments);
                                        }
                                    }
                                    else
                                    {
                                        returnConstraintType = x;
                                    }

                                    return returnConstraintType;
                                }).Where(x => x != null).ToArray();


                                if (isValid)
                                {
                                    Contract.Assume(constraint.IsGenericType);
                                    var constraintGenericDefinition = constraint.GetGenericTypeDefinition();
                                    Contract.Assume(constraintGenericDefinition.IsGenericTypeDefinition);
                                    Contract.Assume(
                                        constraintGenericDefinition.GetGenericArguments().Length
                                        == constraintGenericArguments.Length);

                                    var resolvedConstraintType =
                                        constraintGenericDefinition.MakeGenericType(constraintGenericArguments);
                                    isValid =
                                        resolvedConstraintType.IsAssignableFrom(
                                            topLevelMappedArgumentParameters[genericArgument.Name]);
                                }
                            }
                        }
                        else
                        {
                            if (constraint.IsGenericParameter)
                                isValid = topLevelMappedArgumentParameters[constraint.Name].IsAssignableFrom(topLevelMappedArgumentParameters[genericArgument.Name]);
                            else
                                isValid = constraint.IsAssignableFrom(topLevelMappedArgumentParameters[genericArgument.Name]);
                        }

                        if (!isValid)
                            break;
                    }
                }

                if (!isValid)
                    break;
            }

            return isValid;
        }

        public static bool IsNullable(this Type type)
        {
            return type != null && (Nullable.GetUnderlyingType(type) != null || !type.IsValueType);
        }
    }
}
