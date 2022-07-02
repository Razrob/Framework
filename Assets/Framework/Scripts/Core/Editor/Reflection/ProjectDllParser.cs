using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Framework.Core.Editor
{
    public static class ProjectDllParser
    {
        public static IReadOnlyList<string> TryParseDllNames()
        {
            string solutionPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
            string[] solutionInfo = File.ReadAllLines(Directory.GetFiles(solutionPath, "*.sln").First());

            List<string> dllNames = new List<string>();

            foreach(string row in solutionInfo)
            {
                if(row.Length > 7 && row.Substring(0, 7) is "Project")
                {
                    int equalIndex = row.IndexOf('=');
                    string afterEqual = row.Substring(equalIndex + 1, row.Length - (equalIndex + 1));
                    afterEqual = afterEqual.Trim();

                    if (afterEqual[0] is '\"')
                        afterEqual = afterEqual.Remove(0, 1);

                    dllNames.Add(afterEqual.Substring(0, afterEqual.IndexOf('\"')));
                }
            }

            return dllNames;
        }
    }
}