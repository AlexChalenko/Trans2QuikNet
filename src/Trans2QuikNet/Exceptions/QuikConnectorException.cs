namespace Trans2QuikNet.Exceptions
{
    public class QuikConnectorException : Exception
    {
        public QuikConnectorException(string message) : base(message) { }

        public QuikConnectorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
