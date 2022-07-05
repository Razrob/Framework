using System.Reflection;
using System.Collections.Generic;
using UnityEditor;

namespace Framework.Core.Editor
{
    internal class DllNamesFinder
    {
        private static List<Assembly> _solutionDlls;
        internal static IReadOnlyList<Assembly> SolutionDlls => _solutionDlls;

        [InitializeOnLoadMethod]
        private static void OnRecompile()
        {
            _solutionDlls = new List<Assembly>();
            IReadOnlyList<string> dllNames = ProjectDllParser.TryParseDllNames();

            foreach (string dllName in dllNames)
            {
                Assembly assembly = Assembly.Load(dllName);

                if (assembly is null)
                    continue;

                _solutionDlls.Add(assembly);
            }
        }
    } 
}