namespace Common
{
    public interface IFreeable
    {
        bool IsFreed { get; }
        void Free();
    }
}
