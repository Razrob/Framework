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
    internal class FEntityProviderEditor : UnityEditor.Editor
    {
        private FEntityProvider _entityProvider;
        private SubTypesFinder _subTypesFinder;
        private List<bool> _showedElements;

        private const float AppendButtonHeigth = 35f;
        private const float AppendButtonWidth = 230f;

        private const string EntityBinderName = "_entityBinders";
        private const string InitializationMethodName = "_initializationMethod";

        private const string ComponentFieldName = "_component";
        private const string ComponentProvidersArrayName = "_componentProviders";
        private const string ComponentTypeString = "_componentType";
        private const string ComponentTypeAssemblyString = "_componentTypeAssemblyName";

        private void OnEnable()
        {
            _entityProvider = (FEntityProvider)target;

            if(_showedElements is null) 
                _showedElements = new List<bool>();

            _subTypesFinder = new SubTypesFinder(new Type[] { typeof(FComponent) });
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedProperty componentProvidersArrayProperty = serializedObject.FindProperty(ComponentProvidersArrayName);
            CutList(ref _showedElements, componentProvidersArrayProperty.arraySize, true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty(EntityBinderName));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(InitializationMethodName));

            for (int i = 0; i < componentProvidersArrayProperty.arraySize; i++)
            {
                SerializedProperty arrayElement = componentProvidersArrayProperty.GetArrayElementAtIndex(i);

                string typeName = arrayElement.FindPropertyRelative(ComponentTypeString)?.stringValue; 
                string typeAssemblyName = arrayElement.FindPropertyRelative(ComponentTypeAssemblyString)?.stringValue;
                Type componentType = typeName is null ? null : Assembly.Load(typeAssemblyName).GetType(typeName);

                if(componentType is null)
                    FrameworkDebuger.Log(Runtime.LogType.Info, $"FComponent in {_entityProvider.gameObject.name} not found. Probably it was renamed");

                FieldInfo[] fieldsInfo = null;

                if(componentType != null)
                    fieldsInfo = GetComponentFields(componentType);

                const float menuButtonWidth = 50f;
                Rect menuButtonRect = EditorGUILayout.GetControlRect(GUILayout.Height(-3f));
                Rect componentHeaderRect = EditorGUILayout.GetControlRect(GUILayout.Height(CustomEditorStyles.ComponentHeaderStyle.fixedHeight));

                componentHeaderRect.y++;
                componentHeaderRect.width -= menuButtonWidth + 5f;

                if (componentType != null)
                {
                    if (GUI.Button(componentHeaderRect, new GUIContent(componentType.Name), CustomEditorStyles.ComponentHeaderStyle))
                        _showedElements[i] = !_showedElements[i];
                }
                else
                    GUI.Button(componentHeaderRect, new GUIContent($"<type_not_found>"), CustomEditorStyles.ComponentHeaderStyle);

                GUIStyle style = CustomEditorStyles.ComponentHeaderStyle;
                style.alignment = TextAnchor.MiddleCenter;
                style.padding = new RectOffset(5, 0, 0, 0);

                menuButtonRect.width = menuButtonWidth;
                menuButtonRect.x += EditorGUIUtility.currentViewWidth - menuButtonWidth * 1.5f;
                menuButtonRect.height = style.fixedHeight;

                if (GUI.Button(menuButtonRect, "Menu", style))
                    DrawComponentMenu(i);

                if (componentType is null)
                    continue;

                if (_showedElements[i])
                    DrawComponentFields(fieldsInfo, arrayElement.FindPropertyRelative(ComponentFieldName));
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
            IEnumerable<FieldInfo> fieldInfos = type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Where(field => field.GetCustomAttribute<SerializeField>() != null);

            if (type.BaseType == typeof(FComponent))
                return fieldInfos.ToArray();

            return fieldInfos.Concat(GetComponentFields(type.BaseType)).ToArray();
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

            dropDownTypesWindow.Initialize(rect, _subTypesFinder.SubTypes);
            dropDownTypesWindow.SetExcludedValues(GetAddedComponentsProviders()?.Select(componentProvider => componentProvider.ComponentType));

            dropDownTypesWindow.OnTypeSelect += type =>
            {
                if (ContainsComponent(type))
                    return;

                serializedObject.Update();

                SerializedProperty array = serializedObject.FindProperty(ComponentProvidersArrayName);
                int newElementIndex = array.arraySize;
                array.InsertArrayElementAtIndex(newElementIndex);

                FComponent component = (FComponent)Activator.CreateInstance(type);
                FComponentProvider componentProvider = new FComponentProvider(type, type.Assembly, component);

                array.GetArrayElementAtIndex(newElementIndex).managedReferenceValue = componentProvider;

                serializedObject.ApplyModifiedProperties();

                dropDownTypesWindow.Close();
            };
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