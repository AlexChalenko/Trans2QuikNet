using System.Runtime.InteropServices;
using System.Text;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.Delegates
{

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate Result TRANS2QUIK_SEND_SYNC_TRANSACTION(string lpstTransactionString, ref int pnReplyCode, ref uint pdwTransId, ref ulong pnOrderNum, StringBuilder lpstrResultMessage, uint dwResultMessageSize, ref long pnExtendedErrorCode, StringBuilder lpstErrorMessage, uint dwErrorMessageSize);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate Result TRANS2QUIK_SEND_ASYNC_TRANSACTION(string lpstTransactionString, ref int pnExtendedErrorCode, StringBuilder lpstErrorMessage, uint dwErrorMessageSize);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void TRANS2QUIK_TRANSACTION_REPLY_CALLBACK(Result nTransactionResult, int nTransactionExtendedErrorCode, long nTransactionReplyCode, uint dwTransId, ulong nOrderNum, StringBuilder lpcstrTransactionReplyMessage, nint transReplyDescriptor);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate Result TRANS2QUIK_SET_TRANSACTIONS_REPLY_CALLBACK(TRANS2QUIK_TRANSACTION_REPLY_CALLBACK callback, ref int pnExtendedErrorCode, StringBuilder lpstrErrorMessage, uint dwErrorMessageSize);
}
