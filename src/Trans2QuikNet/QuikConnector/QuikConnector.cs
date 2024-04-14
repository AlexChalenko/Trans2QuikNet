using System.Text;
using Trans2QuikNet.Delegates;
using Trans2QuikNet.Exceptions;
using Trans2QuikNet.Models;

namespace Trans2QuikNet
{
    public class QuikConnector : IQuikConnector, IDisposable
    {
        private readonly ITrans2QuikAPI _api;

        private TRANS2QUIK_CONNECT? _connect;
        private TRANS2QUIK_DISCONNECT? _disconnest;
        private TRANS2QUIK_IS_QUIK_CONNECTED? _isQuikConnected;
        private TRANS2QUIK_IS_DLL_CONNECTED? _isDllConnected;
        private TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK? _setConnectionStatusCallback;
        private TRANS2QUIK_CONNECTION_STATUS_CALLBACK? _connectionDelegate;

        public event EventHandler<ConnectionStatusEventArgs>? OnConnectionStatusChanged;
        private bool disposedValue;
        private readonly StringBuilder _errorMessageBuilder = new(1024);

        public QuikConnector(ITrans2QuikAPI api)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));

            InitializeDelegates();
            RegisterConnectionStatusCallback();
        }

        private void InitializeDelegates()
        {
            //TODO: Add IntPtr.Zero check for each delegate
            _connect = _api.GetDelegate<TRANS2QUIK_CONNECT>("TRANS2QUIK_CONNECT");
            _disconnest = _api.GetDelegate<TRANS2QUIK_DISCONNECT>("TRANS2QUIK_DISCONNECT");
            _isDllConnected = _api.GetDelegate<TRANS2QUIK_IS_DLL_CONNECTED>("TRANS2QUIK_IS_DLL_CONNECTED");
            _isQuikConnected = _api.GetDelegate<TRANS2QUIK_IS_QUIK_CONNECTED>("TRANS2QUIK_IS_QUIK_CONNECTED");
            _setConnectionStatusCallback = _api.GetDelegate<TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK>("TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK");

            _connectionDelegate = new TRANS2QUIK_CONNECTION_STATUS_CALLBACK(ConnectionStatusHandler);
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
            long errorCode = 0;
            _errorMessageBuilder.Clear();
            var result = _setConnectionStatusCallback(_connectionDelegate, ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length);
            if (result == Result.FAILED)
            {
                throw new Exception($"Error setting connection status callback: {_errorMessageBuilder}");
            }
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
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
