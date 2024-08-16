using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trans2QuikNet.Managers;
using Trans2QuikNet.Models;
using Trans2QuikNet.Tools;

namespace MultipleQuiks.ViewModels
{
    public partial class QuikViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _quikPath;

        [ObservableProperty]
        private bool _isConnected;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConnectCommand))]
        private bool _isDLLConneted;

        private Trans2QuikAPI _api;
        private QuikConnector _connection;

        public QuikViewModel(string quikPath)
        {
            QuikPath = quikPath;
            _api = new Trans2QuikAPI(quikPath);
            _connection = new QuikConnector(_api);
            _connection.OnConnectionStatusChanged += Connection_OnConnectionStatusChanged;

            //_connection.Connect();
            IsConnected = _connection.IsQuikConnected().Result == Result.QUIK_CONNECTED;
        }

        private void Connection_OnConnectionStatusChanged(object? sender, ConnectionStatusEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                switch (e.ConnectionEvent)
                {
                    case Result.QUIK_CONNECTED:
                    case Result.QUIK_DISCONNECTED:
                        IsConnected = (e.ConnectionEvent == Result.QUIK_CONNECTED);
                        break;
                    case Result.DLL_CONNECTED:
                    case Result.DLL_DISCONNECTED:
                        IsDLLConneted = (e.ConnectionEvent == Result.DLL_CONNECTED);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });
        }

        [RelayCommand(CanExecute = nameof(CanConnect))]
        private void Connect()
        {
            var connectResult = _connection.Connect();
            IsConnected = _connection.IsQuikConnected().Result == Result.QUIK_CONNECTED;
        }

        private bool CanConnect()
        {
            return !IsDLLConneted;
        }
    }
}
