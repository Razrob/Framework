using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.Core.Editor
{
    public class DropDownTypesWindow : DropDownWindowBase<Type>
    {
        protected override float MaxWindowHeight => 250f;
        protected override float MinWindowHeight => 250f;
        protected override float FieldHeight => 23f;
        protected override float SearchAreaHeight => 20f;

        protected override string ValueToString(Type value) => value.Name;
        protected override bool ValueEquals(Type value1, Type value2) => value1.FullName == value2.FullName;
    }
}