using System.Runtime.InteropServices;

namespace Trans2QuikNet.Tools
{
    /// <summary>
    /// Маршалер для получения строк из Trans2Quik
    /// </summary>
    public class QuikStringMarshaler : ICustomMarshaler
    {
        static readonly QuikStringMarshaler instance = new();
        public static ICustomMarshaler GetInstance(string cookie) => instance;
        public object MarshalNativeToManaged(nint pNativeData) => Marshal.PtrToStringAnsi(pNativeData) ?? string.Empty;
        public nint MarshalManagedToNative(object ManagedObj) => nint.Zero;
        public int GetNativeDataSize() => nint.Size;
        public void CleanUpNativeData(nint pNativeData) { }
        public void CleanUpManagedData(object ManagedObj) { }
    }
}
