using System.Runtime.InteropServices;
using Trans2QuikNet.Models;
using Trans2QuikNet.Tools;

namespace Trans2QuikNet.Delegates
{

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(QuikStringMarshaler))]
    public delegate string TRANS2QUIK_REPLY_STRING(nint descriptor);

    //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
    //[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(FileTimeToDateTimeMarshaler))]
    //public delegate FILETIME TRANS2QUIK_REPLY_DATETIME(nint descriptor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate double TRANS2QUIK_REPLY_DOUBLE(nint descriptor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate long TRANS2QUIK_REPLY_LONG(nint descriptor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate short TRANS2QUIK_REPLY_SHORT(nint descriptor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate ulong TRANS2QUIK_REPLY_ULONG(nint descriptor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int TRANS2QUIK_REPLY_INT(nint descriptor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate IntPtr TRANS2QUIK_REPLY_INTPTR(nint descriptor);

}
