using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public class SubTypesFinder
    {
        private IEnumerable<Type> _baseTypes;
        public IReadOnlyList<Type> SubTypes { get; private set; }

        public SubTypesFinder(IEnumerable<Type> baseTypes)
        {
            _baseTypes = baseTypes;
            SubTypes = FindTypes(baseTypes, _baseTypes.ElementAt(0).Assembly);
        }

        public SubTypesFinder(IEnumerable<Type> baseTypes, Assembly targetAssembly)
        {
            _baseTypes = baseTypes;
            SubTypes = FindTypes(baseTypes, targetAssembly);
        }

        public static Type[] FindTypes(IEnumerable<Type> baseTypes, Assembly assembly)
        {
            if (baseTypes is null)
                throw new NullReferenceException();

            return assembly.GetTypes().Where(type =>
            {
                return baseTypes.All(baseType =>
                {
                    if (baseType.IsClass)
                        return type.IsSubclassOf(baseType);
                    else return type.GetInterface(baseType.ToString()) != null;
                });
            }).ToArray();
        }
    }
}