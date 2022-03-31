using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;

[InternalModel]
public class GameData
{
    [InjectSelector] private ComponentSelector<MovementComponent> _movementComponents;
    [InjectField] private GameObject boxPrefab;
    public int Index;

    public void Debuger()
    {
        Debug.Log(boxPrefab);
    }
}
