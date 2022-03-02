
namespace Framework.Core.Runtime
{
    public delegate void FrameworkEvent();
    public delegate void FrameworkEvent<T1>(T1 t1);
    public delegate void FrameworkEvent<T1, T2>(T1 t1, T2 t2);
}