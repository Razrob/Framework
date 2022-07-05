using System;
using UnityEngine;

namespace Framework.Core.Runtime
{
    internal static class DataLoader
    {
        internal static TValue Load<TValue>(string fileName) where TValue : class
        {
            return Load(fileName, typeof(TValue)) as TValue;
        }

        internal static object Load(string fileName, Type type)
        {
            string json = PlayerPrefs.GetString(fileName);

            if (string.IsNullOrEmpty(json))
                return default;

            return JsonUtility.FromJson(json, type);
        }
    }
}