namespace Trans2QuikNet.Models
{
    public class Trans2QuikResult
    {
        public Result Result { get; }
        public long ExtendedErrorCode { get; }
        public string ErrorMessage { get; }

        public Trans2QuikResult(Result result, long extendedErrorCode, string errorMessage)
        {
            Result = result;
            ExtendedErrorCode = extendedErrorCode;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return $"Result: {Result}, ExtendedErrorCode: {ExtendedErrorCode}, ErrorMessage: {ErrorMessage}";
        }
    }
}
