using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Framework.Core.Runtime
{
    public class FEntityRegister : IBootLoadElement
    {
        private readonly FComponentsRepository _componentsRepository;

        public FEntityRegister(out FComponentsRepository componentsRepository)
        {
            _componentsRepository = componentsRepository = new FComponentsRepository();
        }

        public void RegisterFEntity(FEntityProvider entityProvider)
        {
            FEntity entity = new FEntity(entityProvider);
            FieldInfo attachedFEntityField = FComponentsFieldsExtractor.GetAttachedFEntityFieldInfo();
            MethodInfo onAttachMethodInfo = FComponentsFieldsExtractor.GetExecutableComponentMethodInfo(ExecutableComponentMethodID.OnAttach);

            FieldInfo instantiatedEntityField = typeof(FEntityProvider).GetField(nameof(FEntityProvider.InstantiatedEntity),
                BindingFlags.Instance | BindingFlags.Public);

            instantiatedEntityField.SetValue(entityProvider, entity);

            entity.OnFComponentAdd += _componentsRepository.AddFComponent;
            entity.OnFComponentRemove += _componentsRepository.RemoveFComponent;

            foreach (Type componentType in entity.FComponents.Keys)
            {
                FComponent component = entity.FComponents[componentType];
                
                attachedFEntityField.SetValue(component, entity);
                _componentsRepository.AddFComponent(component);
            }

            foreach(FComponent component in entity.FComponents.Values)
                onAttachMethodInfo.Invoke(component, new object[] { entity });

            entityProvider.StartCoroutine(DestroyEntityProvider(entityProvider));
        }

        public void UnregisterFEntity(FEntity entity)
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

        private IEnumerator DestroyEntityProvider(FEntityProvider entityProvider)
        {
            yield return null;

            UnityEngine.Object.Destroy(entityProvider);
        }

        public void OnBootLoadComplete() { }
    }
}