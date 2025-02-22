﻿namespace Trans2QuikNet.Models
{
    public class ConnectionStatusEventArgs : EventArgs
    {
        public Result ConnectionEvent { get; }
        public int ExtendedErrorCode { get; }
        public string ErrorMessage { get; }

        public ConnectionStatusEventArgs(Result connectionEvent, int extendedErrorCode, string errorMessage)
        {
            ConnectionEvent = connectionEvent;
            ExtendedErrorCode = extendedErrorCode;
            ErrorMessage = errorMessage;
        }
    }
}
