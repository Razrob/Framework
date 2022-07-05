using UnityEngine;

namespace Framework.Core.Runtime
{
    internal static class DataSaver
    {
        internal static void Save(object value, string fileName)
        {
            string json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(fileName, json);
        }
    }
}