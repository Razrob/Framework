using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;

[InternalModel]
public class UserInput : InternalModel
{
    public event Action<KeyCode> OnKeyUp;

    public void KeyWasUp(KeyCode keyCode)
    {
        OnKeyUp?.Invoke(keyCode);
    }

    protected override void OnInject()
    {
        Debug.Log("UserInput was inject");
    }
}
