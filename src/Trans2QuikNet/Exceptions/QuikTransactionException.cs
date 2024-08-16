namespace Trans2QuikNet.Exceptions
{
    public class QuikTransactionException(string message, long errorCode) : Exception(message)
    {
        public long ErrorCode { get; } = errorCode;
    }
}
