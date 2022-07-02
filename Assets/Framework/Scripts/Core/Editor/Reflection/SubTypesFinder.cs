using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Editor
{
    public class SubTypesFinder
    {
        private IEnumerable<Type> _baseTypes;
        public IReadOnlyList<Type> SubTypes { get; private set; }

        public SubTypesFinder(IEnumerable<Type> baseTypes)
        {
            _baseTypes = baseTypes;
            SubTypes = FindTypes(baseTypes);
        }

        public static IReadOnlyList<Type> FindTypes(IEnumerable<Type> baseTypes, Assembly specificAssembly)
        {
            if (baseTypes is null)
                throw new NullReferenceException();

            return specificAssembly.GetTypes().Where(type =>
            {
                return baseTypes.All(baseType =>
                {
                    if (baseType.IsClass)
                        return type.IsSubclassOf(baseType);
                    else return type.GetInterface(baseType.ToString()) != null;
                });
            }).ToArray();
        }

        public static IReadOnlyList<Type> FindTypes(IEnumerable<Type> baseTypes)
        {
            if (baseTypes is null)
                throw new NullReferenceException();

            List<Type> types = new List<Type>();

            foreach (Assembly assembly in DllNamesFinder.SolutionDlls) 
                types.AddRange(FindTypes(baseTypes, assembly));

            return types;
        }
    }
}