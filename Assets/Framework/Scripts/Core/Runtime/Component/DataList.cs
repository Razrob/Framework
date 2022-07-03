
namespace Framework.Core.Runtime
{
    internal abstract class DataList<TBase>
    {
        internal abstract void AddElementBase(TBase value);
        internal abstract void RemoveElementBase(TBase value);
    }
}