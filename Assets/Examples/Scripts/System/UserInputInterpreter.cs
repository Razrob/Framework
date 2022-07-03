using UnityEngine;
using Framework.Core.Runtime;

[FrameworkSystem(State.Menu)]
public class UserInputInterpreter : FSystemFoundation
{
    [InjectModel] private UserInput _userInput;

    [Executable] protected override void OnUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Space))
            _userInput.KeyWasUp(KeyCode.Space);
    }
}
