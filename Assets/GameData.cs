using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;

[InternalModel]
public class GameData : InternalModel
{
    [InjectSelector] private ComponentSelector<MovementComponent> _movementComponents;
    [InjectField] private GameObject boxPrefab;
    public int Index;

    protected override void OnInject()
    {
        Debug.Log(43242343243);
    }

    public void Debuger()
    {
        //Debug.Log(boxPrefab);
    }
}
