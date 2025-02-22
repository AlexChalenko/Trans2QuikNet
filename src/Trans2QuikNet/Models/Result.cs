﻿namespace Trans2QuikNet.Models
{
    public enum Result : int
    {
        SUCCESS = 0,
        FAILED = 1,
        QUIK_TERMINAL_NOT_FOUND = 2,
        DLL_VERSION_NOT_SUPPORTED = 3,
        ALREADY_CONNECTED_TO_QUIK = 4,
        WRONG_SYNTAX = 5,
        QUIK_NOT_CONNECTED = 6,
        DLL_NOT_CONNECTED = 7,
        QUIK_CONNECTED = 8,
        QUIK_DISCONNECTED = 9,
        DLL_CONNECTED = 10,
        DLL_DISCONNECTED = 11,
        MEMORY_ALLOCATION_ERROR = 12,
        WRONG_CONNECTION_HANDLE = 13,
        WRONG_INPUT_PARAMS = 14,
        ERROR_BROKEN_PIPE = 109,
        ERROR_PIPE_BUSY = 231,
        ERROR_NO_DATA = 232,
        ERROR_PIPE_NOT_CONNECTED = 233,
        ERROR_PIPE_CONNECTED = 535,
    }
}
