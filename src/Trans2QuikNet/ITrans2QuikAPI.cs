namespace Trans2QuikNet
{
    public interface ITrans2QuikAPI: IDisposable
    {
        string QuikPath { get; }
        T GetDelegate<T>(string procName) where T : class;
    }
}