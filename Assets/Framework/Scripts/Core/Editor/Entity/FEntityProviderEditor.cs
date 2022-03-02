using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Framework.Core.Runtime;
using System.Reflection;
using System.Linq;

namespace Framework.Core.Editor
{ 
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FEntityProvider))]
    public class FEntityProviderEditor : UnityEditor.Editor
    {
        private FEntityProvider _entityProvider;
        private SubTypesFinder _subTypesFinder;
        private List<bool> _showedElements;

        private const float AppendButtonHeigth = 35f;
        private const float AppendButtonWidth = 230f;
        private const string ComponentFieldName = "_component";
        private const string ComponentProvidersArrayName = "_componentProviders";
        private const string ComponentTypeString = "_componentType";

        private void OnEnable()
        {
            _entityProvider = (FEntityProvider)target;

            if(_showedElements is null) 
                _showedElements = new List<bool>();

            _subTypesFinder = new SubTypesFinder(typeof(FComponent), Assembly.GetAssembly(typeof(FComponent)));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();  

            SerializedProperty componentProvidersArrayProperty = serializedObject.FindProperty(ComponentProvidersArrayName);
            CutList(ref _showedElements, componentProvidersArrayProperty.arraySize, true);

            for (int i = 0; i < componentProvidersArrayProperty.arraySize; i++)
            {
                SerializedProperty arrayElement = componentProvidersArrayProperty.GetArrayElementAtIndex(i);

                Type componentType = Assembly.GetAssembly(typeof(FComponentProvider))
                    .GetType(arrayElement.FindPropertyRelative(ComponentTypeString).stringValue);

                FieldInfo[] fieldsInfo = GetComponentFields(componentType);

                _showedElements[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_showedElements[i], new GUIContent(componentType.Name), 
                    CustomEditorStyles.ComponentHeaderStyle, rect => DrawComponentMenu(i));

                if (_showedElements[i])
                    DrawComponentFields(fieldsInfo, arrayElement.FindPropertyRelative(ComponentFieldName));

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            DrawAppendButton(); 

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAppendButton()
        {
            Rect buttonRect = EditorGUILayout.GetControlRect(GUILayout.Height(0f));
            buttonRect.y += 3f;
            buttonRect.width = AppendButtonWidth;
            buttonRect.x = (EditorGUIUtility.currentViewWidth - AppendButtonWidth) / 2f - 1f;

            Rect menuRect = EditorGUIUtility.GUIToScreenRect(buttonRect.ChangeY(buttonRect.y + AppendButtonHeigth));

            buttonRect.height = AppendButtonHeigth;

            EditorGUILayout.Space(AppendButtonHeigth);

            if (EditorGUI.DropdownButton(buttonRect, new GUIContent("FComponent"), FocusType.Passive,
                CustomEditorStyles.AppendComponentButtonStyle))
                ShowAppendComponentWindow(menuRect);
        }

        private FieldInfo[] GetComponentFields(Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttribute<SerializeField>() != null).ToArray();
        }

        private void DrawComponentFields(FieldInfo[] fieldsInfo, SerializedProperty component)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;

            foreach (FieldInfo fieldInfo in fieldsInfo)
            {
                SerializedProperty serializedProperty = component.FindPropertyRelative(fieldInfo.Name);
                EditorGUILayout.PropertyField(serializedProperty, true);
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void DrawComponentMenu(int componentIndex)
        {
            GenericMenu menu = new GenericMenu(); 
            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                serializedObject.Update();
                serializedObject.FindProperty(ComponentProvidersArrayName).DeleteArrayElementAtIndex(componentIndex);
                serializedObject.ApplyModifiedProperties();
            });

            menu.ShowAsContext();
        }

        private bool ContainsComponent(Type componentType)
        {
            FComponentProvider[] componentProviders = GetAddedComponentsProviders();
            return componentProviders?.Count(componentProvider => componentProvider.ComponentType == componentType) > 0;
        }

        private FComponentProvider[] GetAddedComponentsProviders()
        {
            return typeof(FEntityProvider).GetField(ComponentProvidersArrayName, BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(_entityProvider) as FComponentProvider[];
        }

        private void ShowAppendComponentWindow(Rect rect)
        {
            DropDownTypesWindow dropDownTypesWindow = EditorWindow.GetWindow<DropDownTypesWindow>();

            dropDownTypesWindow.OnTypeSelect += type =>
            {
                if (ContainsComponent(type))
                    return;

                serializedObject.Update();

                SerializedProperty array = serializedObject.FindProperty(ComponentProvidersArrayName);
                int newElementIndex = array.arraySize;
                array.InsertArrayElementAtIndex(newElementIndex);

                FComponent component = (FComponent)Activator.CreateInstance(type);
                FComponentProvider componentProvider = new FComponentProvider(type, component);

                array.GetArrayElementAtIndex(newElementIndex).managedReferenceValue = componentProvider;

                serializedObject.ApplyModifiedProperties();

                dropDownTypesWindow.Close();
            };

            dropDownTypesWindow.Initialize(rect, _subTypesFinder.SubTypes);
            dropDownTypesWindow.SetExcludedValues(GetAddedComponentsProviders()?.Select(componentProvider => componentProvider.ComponentType));
        }

        private void CutList<T>(ref List<T> list, int targetLength, T defaultValue = default)
        {
            if (list.Count > targetLength)
                list = list.GetRange(0, targetLength);
            else if (list.Count < targetLength)
            {
                T[] arr = new T[targetLength - list.Count];
                for (int i = 0; i < targetLength - list.Count; i++)
                    arr[i] = defaultValue;

                list.AddRange(arr);
            }
        }
    }
}