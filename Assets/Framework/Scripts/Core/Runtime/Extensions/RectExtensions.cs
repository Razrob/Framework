using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Runtime
{
    public static class RectExtensions
    {
        public static Rect ChangeX(this Rect rect, float value) =>
            new Rect(value, rect.y, rect.width, rect.height);

        public static Rect ChangeY(this Rect rect, float value) =>
            new Rect(rect.x, value, rect.width, rect.height);

        public static Rect ChangeWidth(this Rect rect, float value) =>
            new Rect(rect.x, rect.y, value, rect.height);

        public static Rect ChangeHeigth(this Rect rect, float value) =>
            new Rect(rect.x, rect.y, rect.width, value);
    }
}