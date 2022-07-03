using System.Collections.Generic;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    internal class StateValidator
    {
        private readonly IReadOnlyList<int> _statesIndexes;
        private readonly Type _mainStateType;

        internal StateValidator(IReadOnlyList<int> statesIndexes, Type type)
        {
            _statesIndexes = statesIndexes;
            _mainStateType = type;
        }

        internal void ValidateSystems(IEnumerable<SystemModule> modules)
        {
            foreach(SystemModule systemModule in modules)
                foreach (SystemData systemData in systemModule.SystemsData)
                    ValidateAttribute(systemData);
        }

        private void ValidateAttribute(SystemData systemData)
        {
            if(_mainStateType.GetEnumValues().Length != _statesIndexes.Count)
                ThrowException(systemData.SystemType);
            
            foreach (int systemStateIndex in systemData.SystemExecuteData.FrameworkSystemAttribute.AttachedStates)
                if (!_statesIndexes.Contains(systemStateIndex))
                    ThrowException(systemData.SystemType);
        }

        private void ThrowException(Type systemType)
        {
            FrameworkDebuger.Log(LogType.Exception, $"[ArgumentException], Incorrect system state. Wrong system: {systemType.Name}. " +
                $"You must use the main state: {_mainStateType.Name}");
        }
    }
}