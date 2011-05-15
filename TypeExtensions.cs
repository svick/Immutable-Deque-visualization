using System;
using System.Linq;
using System.Reflection;

namespace Immutable_Deque_visualization
{
    public static class TypeExtensions
    {
        public static string GetShortName(this Type type)
        {
            string result = type.Name;
            if (type.IsGenericType)
            {
                if (type.Name.Contains('`'))
                {
                    // remove genric indication (e.g. `1)
                    result = result.Substring(0, result.LastIndexOf('`'));
                }

                var genericArguments = type.GetGenericArguments();

                if (genericArguments.Length != 1 || genericArguments[0] != typeof(string))
                    result = string.Format(
                        "{0}<{1}>",
                        result,
                        string.Join(", ",
                                    genericArguments.Select(GetShortName)));
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