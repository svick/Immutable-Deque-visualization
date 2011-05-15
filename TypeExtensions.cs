using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Immutable_Deque_visualization
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> KeywordTypes =
            new Dictionary<Type, string> { { typeof(int), "int" } };

        public static string GetShortName(this Type type)
        {
            if (KeywordTypes.ContainsKey(type))
                return KeywordTypes[type];

            string result = type.Name;
            if (type.IsGenericType)
            {
                if (type.Name.Contains('`'))
                {
                    // remove genric indication (e.g. `1)
                    result = result.Substring(0, result.LastIndexOf('`'));
                }

                result = string.Format(
                    "{0}<{1}>",
                    result,
                    string.Join(", ",
                                type.GetGenericArguments().Select(GetShortName)));
            }

            return result;
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, BindingFlags bindingFlags, params Type[] parameterTypes)
        {
            Func<Type, Type> getGenericTypeDefinition =
                t => t.IsGenericParameter ? typeof(object) : t.IsGenericType ? t.GetGenericTypeDefinition() : t;
            parameterTypes = parameterTypes.Select(getGenericTypeDefinition).ToArray();
            return (from method in type.GetMethods(bindingFlags)
                    where method.Name == name
                    let methodParameterTypes =
                        method.GetParameters().Select(p => getGenericTypeDefinition(p.ParameterType)).ToArray()
                    where methodParameterTypes.SequenceEqual(parameterTypes)
                    select method).SingleOrDefault();
        }
    }
}