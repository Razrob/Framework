using System;

namespace Framework.Core.Runtime
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}