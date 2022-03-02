using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;

[FrameworkSystem(State.Menu)]
public class TestSystem2 : FSystemFoundation
{
    [InternalModel] private GameData _gameData;

    [Executable(-100)]
    protected override void OnBegin()
    {
        Debug.Log(_gameData.Index);
    }

    //[Executable(0, false)]
    //protected override void OnUpdate()
    //{
    //    Debug.Log("Update System2");
    //}
}
