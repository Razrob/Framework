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

        private const string FieldName = "_typeName";
        private const string UndefinedName = "Undefined";

        private DropDownTypesWindow window;

        private bool show;

        private void Init(SerializedProperty property)
        {
            Attribute attribute = fieldInfo.GetCustomAttribute(_requiredAttributeType);

            if (attribute is null)
                return;

            property.serializedObject.Update();

            _subTypesFilter = attribute as SubTypesFilterAttribute;
            _subTypesFinder = new SubTypesFinder(_subTypesFilter.BaseTypes);

            _subTypesNamesArray = _subTypesFinder.SubTypes.Select(type => type.Name).ToArray();
            _selectedType = LoadActiveIndex(property);

            if(_subTypesNamesArray.Length > 0)
                property.FindPropertyRelative(FieldName).stringValue = _selectedType >= 0 
                    ? _subTypesFinder.SubTypes[_selectedType].Name : string.Empty;
            
            property.serializedObject.ApplyModifiedProperties();
        }

        private int LoadActiveIndex(SerializedProperty property)
        {
            string typeName = property.FindPropertyRelative(FieldName).stringValue;
            if (string.IsNullOrEmpty(typeName))
                return -1;

            for (int i = 0; i < _subTypesNamesArray.Length; i++)
                if (_subTypesNamesArray[i] == typeName)
                    return i;

            return -1;
        }

        private void ShowWindow(Rect rect, SerializedProperty property)
        {
            window = EditorWindow.GetWindow<DropDownTypesWindow>();
            window.Initialize(GUIUtility.GUIToScreenRect(rect), _subTypesFinder.SubTypes, _subTypesFilter.AllowUndefined);

            window.OnTypeSelect += type =>
            {
                SerializedProperty typeName = property.FindPropertyRelative(FieldName);
                typeName.serializedObject.Update();
                typeName.stringValue = type?.Name;
                typeName.serializedObject.ApplyModifiedProperties();

                window.Close();
            };
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

            string @string = property.FindPropertyRelative(FieldName).stringValue;
            GUIContent gUIContent;
            if (string.IsNullOrEmpty(@string))
                gUIContent = new GUIContent(UndefinedName);
            else gUIContent = new GUIContent(@string);

            show = EditorGUI.DropdownButton(position, gUIContent, FocusType.Passive);

            if (show)
            {
                ShowWindow(position, property);
            }

            if (EditorGUI.EndChangeCheck())
            {
                SerializedProperty typeName = property.FindPropertyRelative(FieldName);
                if (_subTypesNamesArray.Length > 0)
                {
                    typeName.stringValue = _selectedType >= 0 
                        ? _subTypesFinder.SubTypes[_selectedType].ToString() : string.Empty;
                }
                else typeName.stringValue = string.Empty;
            }

            EditorGUI.EndProperty();
        }
    }
}