using System.Runtime.InteropServices;

namespace Trans2QuikNet.Tools
{
    public class FileTimeToDateTimeMarshaler : ICustomMarshaler
    {
        private static readonly FileTimeToDateTimeMarshaler _instance = new FileTimeToDateTimeMarshaler();

        // Factory method for ICustomMarshaler
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return _instance;
        }

        public void CleanUpManagedData(object ManagedObj)
        {
            // No cleanup necessary for DateTime
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            // No cleanup necessary for IntPtr
        }

        public int GetNativeDataSize()
        {
            // Return size of FILETIME structure
            return 8; // FILETIME consists of two 32-bit values
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            return nint.Zero;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return DateTime.FromFileTime(pNativeData);
        }
    }

}
