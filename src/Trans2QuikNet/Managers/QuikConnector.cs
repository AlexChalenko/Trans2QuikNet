using System.Text;
using Trans2QuikNet.Delegates;
using Trans2QuikNet.Exceptions;
using Trans2QuikNet.Interfaces;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.Managers
{
    public class QuikConnector : IQuikConnector, IDisposable
    {
        private readonly ITrans2QuikAPI _api;

        private TRANS2QUIK_CONNECT? _connect;
        private TRANS2QUIK_DISCONNECT? _disconnect;
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
            try
            {
                RegisterConnectionStatusCallback();
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, clean up resources, etc.)
                Dispose(); // Clean up resources if necessary
                throw new QuikConnectorException("Failed to register connection status callback.", ex);
            }
        }

        private void InitializeDelegates()
        {
            // Initialize delegates with null checks
            _connect = _api.GetDelegate<TRANS2QUIK_CONNECT>("TRANS2QUIK_CONNECT")
                ?? throw new InvalidOperationException("Failed to get TRANS2QUIK_CONNECT delegate.");
            _disconnect = _api.GetDelegate<TRANS2QUIK_DISCONNECT>("TRANS2QUIK_DISCONNECT")
                ?? throw new InvalidOperationException("Failed to get TRANS2QUIK_DISCONNECT delegate.");
            _isDllConnected = _api.GetDelegate<TRANS2QUIK_IS_DLL_CONNECTED>("TRANS2QUIK_IS_DLL_CONNECTED")
                ?? throw new InvalidOperationException("Failed to get TRANS2QUIK_IS_DLL_CONNECTED delegate.");
            _isQuikConnected = _api.GetDelegate<TRANS2QUIK_IS_QUIK_CONNECTED>("TRANS2QUIK_IS_QUIK_CONNECTED")
                ?? throw new InvalidOperationException("Failed to get TRANS2QUIK_IS_QUIK_CONNECTED delegate.");
            _setConnectionStatusCallback = _api.GetDelegate<TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK>("TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK")
                ?? throw new InvalidOperationException("Failed to get TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK delegate.");

            _connectionDelegate = new TRANS2QUIK_CONNECTION_STATUS_CALLBACK(ConnectionStatusHandler);
        }

        public Trans2QuikResult Connect()
        {
            ArgumentNullException.ThrowIfNull(_connect, nameof(_connect));

            long errorCode = 0;
            _errorMessageBuilder.Clear();

            return new Trans2QuikResult(
                _connect(_api.QuikPath, ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length),
                errorCode,
                _errorMessageBuilder.ToString());
        }

        public Trans2QuikResult Disconnect()
        {
            ArgumentNullException.ThrowIfNull(_disconnect, nameof(_disconnect));

            long errorCode = 0;
            _errorMessageBuilder.Clear();
            return new Trans2QuikResult(
                _disconnect(ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length),
                errorCode,
                _errorMessageBuilder.ToString());
        }

        public Trans2QuikResult IsQuikConnected()
        {
            ArgumentNullException.ThrowIfNull(_isQuikConnected, nameof(_isQuikConnected));

            long errorCode = 0;
            _errorMessageBuilder.Clear();

            return new Trans2QuikResult(
                _isQuikConnected(ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length),
                errorCode,
                _errorMessageBuilder.ToString());
        }

        public Trans2QuikResult IsDllConnected()
        {
            ArgumentNullException.ThrowIfNull(_isDllConnected, nameof(_isDllConnected));

            long errorCode = 0;
            _errorMessageBuilder.Clear();

            return new Trans2QuikResult(
                _isDllConnected(ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Length),
                errorCode,
                _errorMessageBuilder.ToString());
        }

        private void RegisterConnectionStatusCallback()
        {
            if (_setConnectionStatusCallback == null)
            {
                throw new InvalidOperationException("The connection status callback delegate (_setConnectionStatusCallback) is not initialized.");
            }

            if (_connectionDelegate == null)
            {
                throw new InvalidOperationException("The connection delegate (_connectionDelegate) is not initialized.");
            }

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
