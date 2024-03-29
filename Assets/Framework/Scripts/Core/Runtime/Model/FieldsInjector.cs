﻿using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using System.Collections;

namespace Framework.Core.Runtime
{
    internal class FieldsInjector : IBootLoadElement
    {
        private readonly Dictionary<Type, UnityEngine.Object> _injections;
        private readonly SystemRegister _systemRegister;
        private readonly InternalModelInjector _internalModelInjector;

        internal FieldsInjector(IEnumerable<UnityEngine.Object> injections)
        {
            _injections = new Dictionary<Type, UnityEngine.Object>(injections.Count());
            _systemRegister = LoadElementAdapter<SystemRegister>.Instance;
            _internalModelInjector = LoadElementAdapter<InternalModelInjector>.Instance;

            foreach(UnityEngine.Object injection in injections)
                _injections.Add(injection.GetType(), injection);

            foreach (SystemModule systemModule in _systemRegister.RegisteredModules)
                InjectFields(systemModule);

            InjectToModel();

            _systemRegister.OnModuleRegister += InjectFields;
            _internalModelInjector.OnModelCreate += InjectToModel;
            _systemRegister.OnModuleUnregister += TryUnloadInjections;
        }

        private void InjectFields(SystemModule systemModule)
        {
            foreach (SystemData data in systemModule.SystemsData)
            {
                foreach (FieldInfo field in data.InjectionsFields)
                {
                    if (_injections.ContainsKey(field.FieldType))
                    {
                        field.SetValue(data.System, _injections[field.FieldType]);
                    }
                }
            }
        }

        private void InjectToModel()
        {
            foreach (InternalModel model in _internalModelInjector.AllModels)
                InjectToModel(model);
        }

        private void InjectToModel(object model)
        {
            foreach (FieldInfo injectionField in InjectionsExtractor.GetInjectionsData(model.GetType()))
                if (_injections.ContainsKey(injectionField.FieldType))
                    injectionField.SetValue(model, _injections[injectionField.FieldType]);
        }

        private void TryUnloadInjections(SystemModule systemModule)
        {

        }

        public void OnBootLoadComplete() { }
    }
}