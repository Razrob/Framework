using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;
using System.Linq;

[FrameworkSystem(State.Menu)]
public class TestSystem1 : FSystemFoundation
{
    [InjectModel] private GameData _gameData;

    [InjectSelector] private ComponentSelector<MovementComponent> _movementComponents;

    [Executable(-110)]
    protected override void OnInitialize()
    {
        //FrameworkCommander.DestroyFEntity(_movementComponents[0].AttachedEntity);
    }

    [Executable(-110)]
    protected override void OnBegin()
    {
        //FrameworkCommander.DestroyFEntity(_movementComponents[0].AttachedEntity, false);
    }

    //[Executable]
    //protected override void OnUpdate()
    //{
    //    foreach (MovementComponent movementComponent in _movementComponents)
    //        movementComponent.Transform.position += Vector3.forward * Time.deltaTime;
    //}

    //[Executable]
    //protected override void OnDisable()
    //{
    //    Debug.Log("Disable");
    //}

    //[Executable]
    //protected override void OnEnable()
    //{
    //    Debug.Log("Enable");
    //}

    //[Executable]
    //protected override void OnDestroy()
    //{
    //    Debug.Log("Destroy");
    //}
}
