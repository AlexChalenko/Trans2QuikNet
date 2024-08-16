namespace Trans2QuikNet.Interfaces;

public interface ITrans2QuikAPI : IDisposable
{
    string QuikPath { get; }
    T GetDelegate<T>(string procName) where T : class;
}