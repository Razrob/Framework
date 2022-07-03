using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core.Runtime;
using System.Linq;

[FrameworkSystem(State.Menu)]
public class MovementSystem : FSystemFoundation
{
    [InjectModel] private UserInput _userInput;
    [InjectSelector] private ComponentSelector<MovementComponent> _movementComponents;

    [InjectField] private GameObject _cubePrefab;

    [Executable] protected override void OnInitialize()
    {
        _userInput.OnKeyUp += OnKeyUp;
    }

    [Executable] protected override void OnUpdate()
    {
        foreach (MovementComponent movementComponent in _movementComponents)
            movementComponent.AttachedEntity.AttachedGameObject.transform.position += movementComponent.Velocity * Time.deltaTime;
    }

    private void OnKeyUp(KeyCode keyCode)
    {
        GameObject.Instantiate(_cubePrefab, Vector3.zero, Quaternion.identity);
    }
}