using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Framework.Core.Runtime
{
    public class StateValidator
    {
        private readonly IReadOnlyList<int> _statesIndexes;
        private readonly Type _mainStateType;

        public StateValidator(IReadOnlyList<int> statesIndexes, Type type)
        {
            _statesIndexes = statesIndexes;
            _mainStateType = type;
        }

        public void ValidateSystems(IEnumerable<SystemModule> modules)
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
            throw new ArgumentException($"Incorrect system state. Wrong system: {systemType.Name}. " +
                $"You must use the main state: {_mainStateType.Name}");
        }
    }
}