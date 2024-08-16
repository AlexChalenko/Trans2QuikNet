global using TradeDescriptor = nint;
using System.Runtime.InteropServices;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.Delegates;

// Delegate for the trade status callback function.
[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate void TRANS2QUIK_TRADE_STATUS_CALLBACK(
    long nMode,
    ulong dNumber,
    ulong nOrderNumber,
    string ClassCode,
    string SecCode,
    double dPrice,
    long nQty,
    double dValue,
    long nIsSell,
    TradeDescriptor tradeDescriptor);

// Delegate for subscribing to trades.
[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
public delegate Result TRANS2QUIK_SUBSCRIBE_TRADES(string classCode, string secCodes);

// Delegate for unsubscribing from trades.
[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate Result TRANS2QUIK_UNSUBSCRIBE_TRADES();

// Delegate for starting the reception of trades.
[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate Result TRANS2QUIK_START_TRADES(TRANS2QUIK_TRADE_STATUS_CALLBACK callback);

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
public delegate long TRANS2QUIK_TRADE_DATE_TIME(nint orderDescriptor, long nTimeType);
