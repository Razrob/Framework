using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using Framework.Core.Runtime;

namespace Framework.Core.Editor
{
    [CustomPropertyDrawer(typeof(SerializableType), true)]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        private SubTypesFinder _subTypesFinder;
        private string[] _subTypesNamesArray;
        
        private readonly Type _requiredAttributeType = typeof(SubTypesFilterAttribute);

        private SubTypesFilterAttribute _subTypesFilter;

        private int _selectedType;

        private const string EmptyTypes = "Empty";
        private const string FieldName = "_typeName";

        private DropDownTypesWindow window;

        private bool show;

        private void Init(SerializedProperty property)
        {
            Attribute attribute = fieldInfo.GetCustomAttribute(_requiredAttributeType);

            if (attribute is null)
                return;

            property.serializedObject.Update();

            _subTypesFilter = attribute as SubTypesFilterAttribute;
            _subTypesFinder = new SubTypesFinder(_subTypesFilter.BaseType, _subTypesFilter.Assembly);
            _subTypesNamesArray = _subTypesFinder.SubTypes.Select(type => type.Name).ToArray();

            _selectedType = LoadActiveIndex(property);

            if(_subTypesNamesArray.Length > 0)
                property.FindPropertyRelative(FieldName).stringValue = _subTypesFinder.SubTypes[_selectedType].Name;
            
            property.serializedObject.ApplyModifiedProperties();
        }

        private int LoadActiveIndex(SerializedProperty property)
        {
            string typeName = property.FindPropertyRelative(FieldName).stringValue;
            if (string.IsNullOrEmpty(typeName))
                return 0;

            for (int i = 0; i < _subTypesNamesArray.Length; i++)
                if (_subTypesNamesArray[i] == typeName)
                    return i;

            return 0;
        }

        private int FindTypeIndex(string type)
        {
            for (int i = 0; i < _subTypesNamesArray.Length; i++)
                if (_subTypesNamesArray[i] == type)
                    return i;

            return 0;
        }

        private void ShowWindow(Rect rect, SerializedProperty property)
        {
            window = EditorWindow.GetWindow<DropDownTypesWindow>();
            
            window.OnTypeSelect += type =>
            {
                SerializedProperty typeName = property.FindPropertyRelative(FieldName);
                typeName.serializedObject.Update();
                typeName.stringValue = type.Name;
                typeName.serializedObject.ApplyModifiedProperties();

                window.Close();
            };

            window.Initialize(GUIUtility.GUIToScreenRect(rect), _subTypesFinder.SubTypes);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            position.height = 18f;

            if (_subTypesFinder is null)
                Init(property);

            if (_subTypesFilter is null)
            {
                EditorGUI.PropertyField(position, property);
                return;
            }

            EditorGUI.BeginChangeCheck();

            //_selectedType = EditorGUI.Popup(position, FindTypeIndex(property.FindPropertyRelative(FieldName).stringValue),
            //    _subTypesNamesArray.Length is 0 ? new string[] { EmptyTypes } : _subTypesNamesArray);
            show = EditorGUI.DropdownButton(position, new GUIContent(property.FindPropertyRelative(FieldName).stringValue), FocusType.Passive);

            if (show)
            {
                ShowWindow(position, property);
            }

            if (EditorGUI.EndChangeCheck())
            {
                SerializedProperty typeName = property.FindPropertyRelative(FieldName);
                if (_subTypesNamesArray.Length > 0)
                    typeName.stringValue = _subTypesFinder.SubTypes[_selectedType].ToString();
                else typeName.stringValue = string.Empty;
            }

            EditorGUI.EndProperty();


        }
    }

}

//public class FindTypesWindow : EditorWindow
//{
//    public Rect Rect;
//    public Vector2 Size;
//    public IReadOnlyList<Type> Types;

//    public event Action<Type> OnTypeSelect;

//    private const float maxSizeY = 250f;

//    private string findText;

//    public void OnEnable()
//    {
//        if (Types != null)
//        {
//            Size.y = Mathf.Clamp((Types.Count() + 1) * 21 + 9f, 0f, maxSizeY);
//        }

//        ShowAsDropDown(Rect, Size);
//    }

//    private void OnGUI()
//    {
//        Rect rect = new Rect(5, 5, Size.x - 10, 21);

//        findText =  EditorGUI.TextField(rect, findText, new GUIStyle("SearchTextField"));

//        GUIStyle style = new GUIStyle("ProjectBrowserHeaderBgTop");
//        style.fontSize = 12;
//        style.richText = true;
//        style.alignment = TextAnchor.MiddleLeft;
//        style.fixedHeight = 21;

//        foreach (Type type in FindSuitableTypes(findText))
//        {
//            rect.y += 21f;
//            if (GUI.Button(rect, new GUIContent(type.Name), style))
//                OnTypeSelect?.Invoke(type);
//        }
//    }

//    private IReadOnlyList<Type> FindSuitableTypes(string findText)
//    {
//        if (findText is null || findText.Length < 1)
//            return Types;

//        findText = findText.ToLower();

//        IReadOnlyList<Type> types = Types.Where(type =>
//        {
//            for (int i = 0; i < findText.Length; i++)
//            {
//                if (type.Name.Length >= i + 1)
//                {
//                    if (type.Name.ToLower()[i] != findText[i])
//                        return false;

//                }
//            }
//            return true;
//        })
//        .ToArray();

//        return types;
//    }
//}