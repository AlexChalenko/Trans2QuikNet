namespace Trans2QuikNet.Exceptions
{
    public class QuikConnectionException : Exception
    {
        public long ErrorCode { get; }

        public QuikConnectionException(string message, long errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
