using System.Text;
using Trans2QuikNet.Delegates;
using Trans2QuikNet.Exceptions;
using Trans2QuikNet.Interfaces;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.Managers
{
    public class QuikTransactionManager : IDisposable, IQuikTransactionManager
    {
        private readonly ITrans2QuikAPI _api;


        private TRANS2QUIK_SEND_SYNC_TRANSACTION? _sendTransaction;
        private TRANS2QUIK_SEND_ASYNC_TRANSACTION? _sendTransactionAsync;
        private TRANS2QUIK_SET_TRANSACTIONS_REPLY_CALLBACK? _setTransactionReplyCallback;

        private TRANS2QUIK_REPLY_STRING? _transactionReplyClassCode;
        private TRANS2QUIK_REPLY_STRING? _transactionReplySecCode;
        private TRANS2QUIK_REPLY_DOUBLE? _transactionReplyPrice;
        private TRANS2QUIK_REPLY_LONG? _transactionReplyQuantity;
        private TRANS2QUIK_REPLY_LONG? _transactionReplyBalance;
        private TRANS2QUIK_REPLY_STRING? _transactionReplyFirmid;
        private TRANS2QUIK_REPLY_STRING? _transactionReplyAccount;
        private TRANS2QUIK_REPLY_STRING? _transactionReplyClientCode;
        private TRANS2QUIK_REPLY_STRING? _transactionReplyBrokerref;
        private TRANS2QUIK_REPLY_STRING? _transactionReplyExchange;

        private readonly StringBuilder _errorMessageBuilder = new(1024);

        private bool disposedValue;
        private readonly TRANS2QUIK_TRANSACTION_REPLY_CALLBACK _transactionReplyCallback;

        public event EventHandler<TransactionReplyEventArgs>? OnTransactionReplyReceived;

        public QuikTransactionManager(ITrans2QuikAPI api)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));

            _transactionReplyCallback = new TRANS2QUIK_TRANSACTION_REPLY_CALLBACK(TransactionReplyHandler);

            InitializeDelegates();
            RegisterTransactionReplyCallback();
        }

        private void InitializeDelegates()
        {
            _sendTransaction = _api.GetDelegate<TRANS2QUIK_SEND_SYNC_TRANSACTION>("TRANS2QUIK_SEND_SYNC_TRANSACTION");
            _sendTransactionAsync = _api.GetDelegate<TRANS2QUIK_SEND_ASYNC_TRANSACTION>("TRANS2QUIK_SEND_ASYNC_TRANSACTION");
            _setTransactionReplyCallback = _api.GetDelegate<TRANS2QUIK_SET_TRANSACTIONS_REPLY_CALLBACK>("TRANS2QUIK_SET_TRANSACTIONS_REPLY_CALLBACK");

            _transactionReplyClassCode = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_CLASS_CODE");
            _transactionReplySecCode = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_SEC_CODE");
            _transactionReplyPrice = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRANSACTION_REPLY_PRICE");
            _transactionReplyQuantity = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRANSACTION_REPLY_QUANTITY");
            _transactionReplyBalance = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRANSACTION_REPLY_BALANCE");
            _transactionReplyFirmid = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_FIRMID");
            _transactionReplyAccount = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_ACCOUNT");
            _transactionReplyClientCode = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_CLIENT_CODE");
            _transactionReplyBrokerref = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_BROKERREF");
            _transactionReplyExchange = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_EXCHANGE_CODE");
        }

        private void RegisterTransactionReplyCallback()
        {
            int errorCode = 0;
            _errorMessageBuilder.Clear();
            var result = _setTransactionReplyCallback(_transactionReplyCallback, ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Capacity);
            if (result != Result.SUCCESS)
            {
                throw new QuikTransactionException($"Error setting transaction reply callback: {_errorMessageBuilder}", errorCode);
            }
        }

        private void TransactionReplyHandler(Result nTransactionResult, int nTransactionExtendedErrorCode, long nTransactionReplyCode, uint dwTransId, ulong nOrderNum, StringBuilder lpcstrTransactionReplyMessage, nint transReplyDescriptor)
        {
            //var classCode = _transactionReplyClassCode(transReplyDescriptor);
            //var secCode = _transactionReplySecCode(transReplyDescriptor);
            //var price = _transactionReplyPrice(transReplyDescriptor);
            //var quantity = _transactionReplyQuantity(transReplyDescriptor);
            //var balance = _transactionReplyBalance(transReplyDescriptor);
            //var firmId = _transactionReplyFirmid(transReplyDescriptor);
            //var account = _transactionReplyAccount(transReplyDescriptor);
            //var clientCode = _transactionReplyClientCode(transReplyDescriptor);
            //var brokerRef = _transactionReplyBrokerref(transReplyDescriptor);
            //var exchange = _transactionReplyExchange(transReplyDescriptor);

            OnTransactionReplyReceived?.Invoke(this, new TransactionReplyEventArgs(nTransactionResult, nTransactionExtendedErrorCode, nTransactionReplyCode, dwTransId, nOrderNum, lpcstrTransactionReplyMessage.ToString()));
        }

        public Trans2QuikTransactionResult SendTransaction(Transaction transaction)
        {
            long errorCode = 0;
            int replyCode = 0;
            uint transactionId = 0;
            EntityNumber orderNum = 0;
            var resultMessage = new StringBuilder(1024);
            _errorMessageBuilder.Clear();

            if (_sendTransaction?.Invoke(transaction.ToString(),
                                         ref replyCode,
                                         ref transactionId,
                                         ref orderNum,
                                         resultMessage,
                                         (uint)resultMessage.Capacity,
                                         ref errorCode,
                                         _errorMessageBuilder,
                                         (uint)_errorMessageBuilder.Capacity) is { } result)
            {
                return new Trans2QuikTransactionResult(result,
                                                       replyCode,
                                                       transactionId,
                                                       orderNum,
                                                       resultMessage.ToString(),
                                                       errorCode,
                                                       _errorMessageBuilder.ToString());
            }

            throw new ArgumentNullException(nameof(_sendTransaction));
        }

        public Trans2QuikResult SendTranactionAsync(Transaction transaction)
        {
            int errorCode = 0;
            _errorMessageBuilder.Clear();
            return new Trans2QuikResult(_sendTransactionAsync(transaction.ToString(),
                                                              ref errorCode,
                                                              _errorMessageBuilder,
                                                              (uint)_errorMessageBuilder.Capacity), errorCode, _errorMessageBuilder.ToString());
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

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~QuikTransactionManager()
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
