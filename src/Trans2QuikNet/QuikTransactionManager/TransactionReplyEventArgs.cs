using Trans2QuikNet.Models;

namespace Trans2QuikNet
{
    public class TransactionReplyEventArgs : EventArgs
    {
        public Result TransactionResult { get; }
        public int ExtendedErrorCode { get; }
        public long TransactionReplyCode { get; }
        public uint TransactionId { get; }
        public ulong OrderNum { get; }
        public string TransactionReplyMessage { get; }


        public TransactionReplyEventArgs(Result transactionResult, int extendedErrorCode, long transactionReplyCode, uint transactionId, ulong orderNum, string transactionReplyMessage)
        {
            TransactionResult = transactionResult;
            ExtendedErrorCode = extendedErrorCode;
            TransactionReplyCode = transactionReplyCode;
            TransactionId = transactionId;
            OrderNum = orderNum;
            TransactionReplyMessage = transactionReplyMessage;
        }

        public override string ToString()
        {
            return $"TransactionResult: {TransactionResult}, ExtendedErrorCode: {ExtendedErrorCode}, TransactionReplyCode: {TransactionReplyCode}, TransactionId: {TransactionId}, OrderNum: {OrderNum}, TransactionReplyMessage: {TransactionReplyMessage}";
        }
    }
}
