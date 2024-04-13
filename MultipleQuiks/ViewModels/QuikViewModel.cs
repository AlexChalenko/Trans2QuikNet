using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Trans2QuikNet;
using Trans2QuikNet.Models;

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

            try
            {

                //_connection.Connect();
                IsConnected = _connection.IsQuikConnected().Result == Result.DLL_CONNECTED;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Connection_OnConnectionStatusChanged(object? sender, ConnectionStatusEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (e.ConnectionEvent.Equals(Result.DLL_CONNECTED))
                {
                    IsDLLConneted = true;
                }
                else if (e.ConnectionEvent.Equals(Result.DLL_DISCONNECTED))
                {
                    IsDLLConneted = false;
                }
                else if (e.ConnectionEvent.Equals(Result.QUIK_CONNECTED))
                {
                    IsConnected = true;
                }
                else if (e.ConnectionEvent.Equals(Result.QUIK_DISCONNECTED))
                {
                    IsConnected = false;
                }
            });
        }

        [RelayCommand(CanExecute = nameof(CanConnect))]
        private void Connect()
        {
            try
            {
                _connection.Connect();
                //IsConnected = _connection.IsQuikConnected();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CanConnect()
        {
            return !IsDLLConneted;
        }
    }
}
