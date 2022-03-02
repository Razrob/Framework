using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;

[Serializable]
public class InputComponent : FComponent
{
    [SerializeField] public Vector2 CurrentOffcet;
    [SerializeField] public Quaternion Rotation;
    [SerializeField] public Rect Rect;
    [SerializeField] public GameObject GameObject;
}
