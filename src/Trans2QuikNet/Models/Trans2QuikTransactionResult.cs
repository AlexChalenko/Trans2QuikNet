namespace Trans2QuikNet.Models
{
    public class Trans2QuikTransactionResult : Trans2QuikResult
    {
        public long ReplyCode { get; }
        public long TransId { get; }
        public double OrderNum { get; }
        public string ResultMessage { get; }

        public Trans2QuikTransactionResult(Result result, long replyCode, long transId, double orderNum, string resultMessage, long extendedErrorCode, string errorMessage) : base(result, extendedErrorCode, errorMessage)
        {
            ReplyCode = replyCode;
            TransId = transId;
            OrderNum = orderNum;
            ResultMessage = resultMessage;
        }

        public override string ToString()
        {
            return $"TransId: {TransId}, OrderNum: {OrderNum}, ReplyCode: {ReplyCode}, ResultMessage: {ResultMessage}, {base.ToString()}";
        }
    }
}
