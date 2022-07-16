using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Framework.Core.Runtime
{
    internal class FEntityRegister : IBootLoadElement
    {
        private readonly FComponentsRepository _componentsRepository;

        private readonly FEvent<FEntity> _onFEntityRegister = new FEvent<FEntity>();
        private readonly Dictionary<Type, FEvent<FComponent>> _fComponentRegisterEvents = new Dictionary<Type, FEvent<FComponent>>();

        internal IEventListener<FEntity> OnFEntityRegister => _onFEntityRegister;

        internal FEntityRegister(out FComponentsRepository componentsRepository)
        {
            _componentsRepository = componentsRepository = new FComponentsRepository();
        }

        internal void RegisterFEntity(FEntityProvider entityProvider)
        {
            FEntity entity = new FEntity(entityProvider);
            FieldInfo attachedFEntityField = FComponentsFieldsExtractor.GetAttachedFEntityFieldInfo();
            FieldInfo fcompoentnTypeField = FComponentsFieldsExtractor.GetFComponentTypeFieldInfo();
            MethodInfo onAttachMethodInfo = FComponentsFieldsExtractor.GetExecutableComponentMethodInfo(ExecutableComponentMethodID.OnAttach);

            FieldInfo instantiatedEntityField = typeof(FEntityProvider).GetField(nameof(FEntityProvider.InstantiatedEntity),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            instantiatedEntityField.SetValue(entityProvider, entity);

            foreach (Type componentType in entity.FComponents.Keys)
            {
                FComponent component = entity.FComponents[componentType];
                attachedFEntityField.SetValue(component, entity);
                fcompoentnTypeField.SetValue(component, componentType);
            }

            foreach (FComponent component in entity.FComponents.Values)
            {
                onAttachMethodInfo.Invoke(component, new object[] { entity });
                OnFComponentRegister(component);
            }

            _onFEntityRegister.Invoke(entity);
            entityProvider.StartCoroutine(DestroyEntityProvider(entityProvider));
        }

        internal void UnregisterFEntity(FEntity entity)
        {
            entity.OnFComponentAdd -= _componentsRepository.AddFComponent;
            entity.OnFComponentRemove -= _componentsRepository.RemoveFComponent;

            MethodInfo onDetachMethodInfo = FComponentsFieldsExtractor.GetExecutableComponentMethodInfo(ExecutableComponentMethodID.OnDetach);

            foreach (Type componentType in entity.FComponents.Keys)
            {
                FComponent component = entity.FComponents[componentType];

                _componentsRepository.RemoveFComponent(component);

                onDetachMethodInfo.Invoke(component, new object[] { entity });
            }
        }

        internal FEvent<FComponent> GetFComponentRegisterEvent<TFComponent>()
        {
            if(_fComponentRegisterEvents.TryGetValue(typeof(TFComponent), out FEvent<FComponent> fEvent))
                return fEvent;

            FEvent<FComponent> @event = new FEvent<FComponent>();
            _fComponentRegisterEvents.Add(typeof(TFComponent), @event);
            return @event;
        }

        private void OnFComponentRegister(FComponent component)
        {
            if (_fComponentRegisterEvents.TryGetValue(component.FComponentType, out FEvent<FComponent> fEvent))
                fEvent.Invoke(component);
        }

        private IEnumerator DestroyEntityProvider(FEntityProvider entityProvider)
        {
            yield return null;

            UnityEngine.Object.Destroy(entityProvider);
        }

        public void OnBootLoadComplete() { }
    }
}