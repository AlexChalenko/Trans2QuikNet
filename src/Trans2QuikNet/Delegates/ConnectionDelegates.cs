using System.Runtime.InteropServices;
using System.Text;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.Delegates
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate Result TRANS2QUIK_CONNECT(string lpcstrConnectionParamsString, ref long pnExtendedErrorCode, StringBuilder lpstrErrorMessage, uint dwErrorMessageSize);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate Result TRANS2QUIK_DISCONNECT(ref long pnExtendedErrorCode, StringBuilder lpstrErrorMessage, uint dwErrorMessageSize);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate Result TRANS2QUIK_IS_QUIK_CONNECTED(ref long pnExtendedErrorCode, StringBuilder lpstrErrorMessage, uint dwErrorMessageSize);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate Result TRANS2QUIK_IS_DLL_CONNECTED(ref long pnExtendedErrorCode, StringBuilder lpstrErrorMessage, uint dwErrorMessageSize);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void TRANS2QUIK_CONNECTION_STATUS_CALLBACK(Result nConnectionEvent, int nExtendedErrorCode, string lpcstrInfoMessage);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate Result TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK(TRANS2QUIK_CONNECTION_STATUS_CALLBACK callback, ref long pnExtendedErrorCode, StringBuilder lpstrErrorMessage, uint dwErrorMessageSize);
}
