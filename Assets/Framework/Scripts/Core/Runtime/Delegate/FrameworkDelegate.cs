
namespace Framework.Core.Runtime
{
    public delegate void FrameworkDelegate();
    public delegate void FrameworkDelegate<T1>(T1 t1);
    public delegate void FrameworkDelegate<T1, T2>(T1 t1, T2 t2);
}