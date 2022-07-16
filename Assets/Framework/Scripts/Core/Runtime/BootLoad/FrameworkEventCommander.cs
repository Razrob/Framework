
namespace Framework.Core.Runtime
{
    public class FrameworkEventCommander
    {
        private static FEntityRegister _entityRegister;

        private static readonly FEvent<FEntity> _onFEntityRegister = new FEvent<FEntity>();
        public static IEventListener<FEntity> OnFEntityRegister => _onFEntityRegister;

        internal static void Initialize()
        {
            _entityRegister = LoadElementAdapter<FEntityRegister>.Instance;

            _entityRegister.OnFEntityRegister.AddListener(entity => _onFEntityRegister.Invoke(entity));
        }

        public static IEventListener<FComponent> GetFComponentRegisterEvent<TFComponent>() => _entityRegister.GetFComponentRegisterEvent<TFComponent>();
    }
}
