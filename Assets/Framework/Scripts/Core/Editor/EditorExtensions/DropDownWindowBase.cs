using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;

namespace Framework.Core.Editor
{
    internal abstract class DropDownWindowBase<T> : EditorWindow where T : class
    {
        private Rect _rect;
        private Vector2 _size;
        private IReadOnlyList<T> _dropDownValues;

        private IEnumerable<T> _excludedValues;

        protected abstract float MaxWindowHeight { get; }
        protected abstract float MinWindowHeight { get; }
        protected abstract float FieldHeight { get; }
        protected abstract float SearchAreaHeight { get; }

        private bool _allowUndefined;

        public event Action<T> OnTypeSelect;

        private string _findText;

        private int _lastSuitableValuesCount;

        internal void Initialize(Rect rect, IReadOnlyList<T> dropDownValues, bool allowUndefined = false)
        {
            OnTypeSelect = null;

            _allowUndefined = allowUndefined;

            _dropDownValues = dropDownValues;

            _size.x = rect.width;
            _rect = rect;

            RefreshWindowHeigth(0);
        }

        internal void SetExcludedValues(IEnumerable<T> values) => _excludedValues = values;

        protected abstract string ValueToString(T value);
        protected virtual bool ValueEquals(T value1, T value2) => false;

        private void OnEnable()
        {
            ShowAsDropDown(_rect, _size);
        }

        private void RefreshWindowHeigth(int fieldCount)
        {
            _size.y = Mathf.Clamp(fieldCount * FieldHeight + SearchAreaHeight + 13f, MinWindowHeight, MaxWindowHeight);
            ShowAsDropDown(_rect, _size);
        }

        private void OnGUI()
        {
            if (_dropDownValues is null)
                return;

            IReadOnlyList<T> suitableValues = FindSuitableValues(_findText, _excludedValues);

            EditorGUILayout.Space();

            _findText = EditorGUILayout.TextField(_findText, CustomEditorStyles.GetSearchAreaStyle(SearchAreaHeight, 
                _size.x - 10, new RectOffset(5, 0, 0, 0)));

            if (_lastSuitableValuesCount != suitableValues.Count)
            {
                _lastSuitableValuesCount = suitableValues.Count;
                RefreshWindowHeigth(_lastSuitableValuesCount);
            }

            EditorGUILayout.Space(7.5f);

            if (_allowUndefined)
                if (GUILayout.Button(new GUIContent("Undefined"), CustomEditorStyles.GetDropDownListElementsStyle(FieldHeight)))
                    OnTypeSelect?.Invoke(null);

            foreach (T value in suitableValues)
            {
                if (GUILayout.Button(new GUIContent(ValueToString(value)), CustomEditorStyles.GetDropDownListElementsStyle(FieldHeight)))
                    OnTypeSelect?.Invoke(value);
            }

            Rect lineRect = EditorGUILayout.GetControlRect(GUILayout.Height(1f));
            lineRect.width += 12f;
            lineRect.x -= 6f;
            lineRect.y -= 4f;

            EditorGUI.DrawRect(lineRect, CustomEditorStyles.GetGreyTone(0.1f));
        }

        private IReadOnlyList<T> FindSuitableValues(string findText, IEnumerable<T> excludeValues = null)
        {
            findText = findText?.ToLower();

            IReadOnlyList<T> values = _dropDownValues.Where(value =>
            {
                if (excludeValues != null && excludeValues.Count(excludeValue => ValueEquals(excludeValue, value)) > 0)
                    return false;

                if (findText is null || findText.Length == 0)
                    return true;

                for (int i = 0; i < findText.Length; i++)
                    if (ValueToString(value).Length >= i + 1)
                        if (ValueToString(value).ToLower()[i] != findText[i])
                            return false;

                return true;
            })
            .ToArray();

            return values;
        }
    }
}