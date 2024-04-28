global using OrderDescriptor = nint;
using System.Runtime.InteropServices;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.Delegates
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void TRANS2QUIK_ORDER_STATUS_CALLBACK(int nMode, uint dwTransID, ulong dNumber, string ClassCode, string SecCode, double dPrice, long nBalance, double dValue, long nIsSell, long nStatus, nint orderDescriptor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate Result TRANS2QUIK_SUBSCRIBE_ORDERS(string classCode, string secCodes);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate Result TRANS2QUIK_START_ORDERS(TRANS2QUIK_ORDER_STATUS_CALLBACK callback);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate Result TRANS2QUIK_UNSUBSCRIBE_ORDERS();

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate long TRANS2QUIK_ORDER_DATE_TIME(OrderDescriptor orderDescriptor, long nTimeType);
}
