using UnityEngine;

namespace Framework.Core.Editor
{
    internal static class CustomEditorStyles
    {
        internal static readonly Color ComponentHeaderColor = GetGreyTone(50);

        internal static GUIStyle AppendComponentButtonStyle
        {
            get
            {
                GUIStyle style = new GUIStyle("button");
                style.alignment = TextAnchor.MiddleCenter;

                return style;
            }
        }

        internal static GUIStyle ComponentHeaderStyle
        {
            get
            {
                GUIStyle style = new GUIStyle("foldoutHeader");
                style.richText = true;
                style.fontSize = 14;
                style.fixedHeight = 23f;

                Texture2D greyTexture = new Texture2D(1, 1);
                greyTexture.SetPixel(1, 1, ComponentHeaderColor);
                greyTexture.Apply();

                style.normal.background = greyTexture;

                return style;
            }
        }

        internal static GUIStyle GetDropDownListElementsStyle(float fieldHeight)
        {
            GUIStyle style = new GUIStyle("IN Title");
            style.fontSize = 12;
            style.richText = true;
            style.alignment = TextAnchor.MiddleLeft;
            style.fixedHeight = fieldHeight;
            style.padding = new RectOffset(20, 0, 0, 0);

            return style;
        }

        internal static GUIStyle GetSearchAreaStyle(float heigth, float width, RectOffset margin = default)
        {
            GUIStyle style = new GUIStyle("SearchTextField");
            style.fixedHeight = heigth;
            style.fixedWidth = width;

            if (margin != null)
                style.margin = margin;

            return style;
        }

        internal static Color GetGreyTone(float value) => new Color(value, value, value);
        internal static Color GetGreyTone(int value) => new Color(value / 256f, value / 256f, value / 256f);
    }
}