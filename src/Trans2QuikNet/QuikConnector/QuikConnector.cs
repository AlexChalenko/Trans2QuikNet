using System.Runtime.InteropServices;
using System.Text;
using Trans2QuikNet.Delegates;
using Trans2QuikNet.Exceptions;
using Trans2QuikNet.Models;

namespace Trans2QuikNet
{
    public class QuikConnector : IQuikConnector, IDisposable
    {
        private readonly Trans2QuikAPI _api;

        private TRANS2QUIK_CONNECT? _connect;
        private TRANS2QUIK_DISCONNECT? _disconnest;
        private TRANS2QUIK_IS_QUIK_CONNECTED? _isQuikConnected;
        private TRANS2QUIK_IS_DLL_CONNECTED? _isDllConnected;
        private TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK? _setConnectionStatusCallback;
        private TRANS2QUIK_CONNECTION_STATUS_CALLBACK _connectionDelegate;

        private GCHandle? _connectionStatusHandlerHandle;

        public EventHandler<ConnectionStatusEventArgs>? OnConnectionStatusChanged;
        private bool disposedValue;
        private readonly StringBuilder _errorMessageBuilder = new StringBuilder(1024);

        public QuikConnector(Trans2QuikAPI api)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));

            InitializeDelegates();
            RegisterConnectionStatusCallback();
        }

        private void InitializeDelegates()
        {
            //TODO: Add IntPtr.Zero check for each delegate
            _connect = GetDelegate<TRANS2QUIK_CONNECT>("TRANS2QUIK_CONNECT");
            _disconnest = GetDelegate<TRANS2QUIK_DISCONNECT>("TRANS2QUIK_DISCONNECT");
            _isDllConnected = GetDelegate<TRANS2QUIK_IS_DLL_CONNECTED>("TRANS2QUIK_IS_DLL_CONNECTED");
            _isQuikConnected = GetDelegate<TRANS2QUIK_IS_QUIK_CONNECTED>("TRANS2QUIK_IS_QUIK_CONNECTED");
            _setConnectionStatusCallback = GetDelegate<TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK>("TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK");
        }

        public Trans2QuikResult Connect()
        {
            ArgumentNullException.ThrowIfNull(_connect, nameof(_connect));

            long errorCode = 0;
            _errorMessageBuilder.Clear();

            return new Trans2QuikResult(_connect(_api.QuikPath, ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length),
                                        errorCode,
                                        _errorMessageBuilder.ToString());
        }

        public Trans2QuikResult Disconnect()
        {
            ArgumentNullException.ThrowIfNull(_disconnest, nameof(_disconnest));

            long errorCode = 0;
            _errorMessageBuilder.Clear();
            return new Trans2QuikResult(_disconnest(ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length),
                                        errorCode,
                                        _errorMessageBuilder.ToString());
        }

        public Trans2QuikResult IsQuikConnected()
        {
            ArgumentNullException.ThrowIfNull(_isQuikConnected, nameof(_isQuikConnected));

            long errorCode = 0;
            _errorMessageBuilder.Clear();

            return new Trans2QuikResult(_isQuikConnected(ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length),
                                                       errorCode,
                                                       _errorMessageBuilder.ToString());
        }

        public Trans2QuikResult IsDllConnected()
        {
            ArgumentNullException.ThrowIfNull(_isDllConnected, nameof(_isDllConnected));

            long errorCode = 0;
            _errorMessageBuilder.Clear();

            return new Trans2QuikResult(_isDllConnected(ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length),
                                                       errorCode,
                                                       _errorMessageBuilder.ToString());
        }

        private void RegisterConnectionStatusCallback()
        {
            ArgumentNullException.ThrowIfNull(_setConnectionStatusCallback, nameof(_setConnectionStatusCallback));

            _connectionDelegate = new TRANS2QUIK_CONNECTION_STATUS_CALLBACK(ConnectionStatusHandler);
            long errorCode = 0;
            _errorMessageBuilder.Clear();
            var result = _setConnectionStatusCallback(_connectionDelegate, ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length);
            if (result == Result.FAILED)
            {
                throw new Exception($"Error setting connection status callback: {_errorMessageBuilder}");
            }
            //_connectionStatusHandlerHandle = GCHandle.Alloc(_connectionDelegate);
        }

        private void ConnectionStatusHandler(Result nConnectionEvent, int nExtendedErrorCode, string lpcstrInfoMessage)
        {
            var handler = OnConnectionStatusChanged;
            if (handler != null)
            {
                try
                {
                    handler(this, new ConnectionStatusEventArgs(nConnectionEvent, nExtendedErrorCode, lpcstrInfoMessage));
                }
                catch (Exception ex)
                {
                    throw new QuikConnectionException($"Error in connection status handler: {ex}", 0);
                }
            }
        }

        private T GetDelegate<T>(string procName) where T : class
        {
            var ptr = _api.GetProcAddress(procName);
            if (ptr == IntPtr.Zero) throw new InvalidOperationException($"PROC not found: {procName}");
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                if (_connectionStatusHandlerHandle.HasValue && _connectionStatusHandlerHandle.Value.IsAllocated)
                {
                    _connectionStatusHandlerHandle?.Free();
                }
                _api?.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        ~QuikConnector()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
