using System.Collections.Generic;
using Framework.Core.Runtime;
using System.Linq;
using UnityEngine;
using System.IO;

namespace Framework.Core.Editor
{
    internal static class ProjectDllParser
    {
        internal static IReadOnlyList<string> TryParseDllNames()
        {
            string solutionPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));

            string[] slnFilesPaths = Directory.GetFiles(solutionPath, "*.sln");

            if (slnFilesPaths.Length is 0)
                FrameworkDebuger.Log(Runtime.LogType.Exception, ".sln file not found in root folder of the project");

            string[] solutionInfo = File.ReadAllLines(slnFilesPaths.OrderBy(path => File.GetLastWriteTime(path).Ticks).Last());

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