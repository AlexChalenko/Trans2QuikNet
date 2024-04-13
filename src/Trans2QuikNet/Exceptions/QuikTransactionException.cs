namespace Trans2QuikNet.Exceptions
{
    public class QuikTransactionException : Exception
    {
        public long ErrorCode { get; }

        public QuikTransactionException(string message, long errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }

}
