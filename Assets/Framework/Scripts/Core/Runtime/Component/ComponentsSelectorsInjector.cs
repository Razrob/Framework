using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    internal class ComponentsSelectorsInjector : IBootLoadElement
    {
        private readonly FComponentsRepository _componentsRepository;
        private readonly InternalModelInjector _internalModelInjector;
        private readonly BindingFlags ConstructorFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        internal ComponentsSelectorsInjector(FComponentsRepository componentsRepository)
        {
            _componentsRepository = componentsRepository;

            _internalModelInjector = LoadElementAdapter<InternalModelInjector>.Instance;
            SystemRegister systemRegister = LoadElementAdapter<SystemRegister>.Instance;

            foreach (SystemModule systemModule in systemRegister.RegisteredModules)
                InjectToSystems(systemModule);

            InjectToModel();

            systemRegister.OnModuleRegister += InjectToSystems;
            _internalModelInjector.OnModelCreate += InjectToModel;
        }

        private void InjectToSystems(SystemModule systemModule)
        {
            foreach (SystemData data in systemModule.SystemsData)
            {
                foreach (FieldInfo field in data.ComponentSelectorFields)
                {
                    field.SetValue(data.System, Activator.CreateInstance(field.FieldType, 
                        ConstructorFlags, null, new object[] { _componentsRepository }, null));
                }
            }
        }

        private void InjectToModel()
        {
            foreach (Type modelType in _internalModelInjector.Models.Keys)
            {
                InjectToModel(_internalModelInjector.Models[modelType]);
            }
        }

        private void InjectToModel(object model)
        {
            foreach(FieldInfo selectorField in ComponentSelectorExtractor.GetSelectors(model.GetType()))
                selectorField.SetValue(model, Activator.CreateInstance(selectorField.FieldType,
                    ConstructorFlags, null, new object[] { _componentsRepository }, null));
        }

        public void OnBootLoadComplete() { }
    }
}