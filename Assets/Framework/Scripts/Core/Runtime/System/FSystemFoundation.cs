using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public abstract class FSystemFoundation : IExecutable
    {
        [Executable(false)] protected virtual void OnInitialize() { }
        [Executable(false)] protected virtual void OnBegin() { }

        [Executable(false)] protected virtual void OnUpdate() { }
        [Executable(false)] protected virtual void OnFixedUpdate() { }

        [Executable(false)] protected virtual void OnEnable() { }
        [Executable(false)] protected virtual void OnDisable() { }

        [Executable(false)] protected virtual void OnDestroy() { }
    }
}