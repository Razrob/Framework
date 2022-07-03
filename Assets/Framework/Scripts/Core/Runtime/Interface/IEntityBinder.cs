
namespace Framework.Core.Runtime
{
    public interface IEntityBinder
    {
        public FEntity BindedEntity { get; }
        public void BindEntity(FEntity entity);
    }
}