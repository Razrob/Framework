using System;

namespace Framework.Core.Runtime
{
    public class FrameworkEventCommander
    {
        private static readonly FEvent<FEntity> _onFEntityRegister = new FEvent<FEntity>();
        private static readonly FEvent<FComponent> _onFComponentRegister = new FEvent<FComponent>();

        public static IEventListener<FEntity> OnFEntityRegister;
        public static IEventListener<FComponent> OnFComponentRegister;

        internal static void Initialize()
        {
            FEntityRegister entityRegister = LoadElementAdapter<FEntityRegister>.Instance;

            entityRegister.OnFEntityRegister.AddListener(entity => _onFEntityRegister.Invoke(entity));
            entityRegister.OnFComponentRegister.AddListener(component => _onFComponentRegister.Invoke(component));
        }
    }
}
