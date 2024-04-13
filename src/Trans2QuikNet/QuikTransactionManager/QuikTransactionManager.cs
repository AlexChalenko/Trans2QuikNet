using System.Runtime.InteropServices;
using System.Text;
using Trans2QuikNet.Delegates;
using Trans2QuikNet.Exceptions;
using Trans2QuikNet.Models;

namespace Trans2QuikNet
{
    public class QuikTransactionManager : IDisposable, IQuikTransactionManager
    {
        private readonly Trans2QuikAPI _api;


        private TRANS2QUIK_SEND_SYNC_TRANSACTION _sendTransaction;
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


        //private TRANS2QUIK_REPLY_SHORT _trans2quikTransactionReplyQtyScale;
        //private TRANS2QUIK_REPLY_SHORT _transactionReplyOrdersCount;
        //private TRANS2QUIK_REPLY_ULONG _transactionReplyFirstOrderNumberById;
        //private TRANS2QUIK_REPLY_ULONG _transactionReplyOrderNumberById;
        //private TRANS2QUIK_REPLY_DOUBLE _transactionReplyPriceById;
        //private TRANS2QUIK_REPLY_LONG _transactionReplyQuantityById;
        //private TRANS2QUIK_REPLY_LONG _transactionReplyBalanceById;
        //private TRANS2QUIK_REPLY_STRING _transactionReplyFirmidById;
        //private TRANS2QUIK_REPLY_STRING _transactionReplyAccountById;
        //private TRANS2QUIK_REPLY_STRING _transactionReplyClientCodeById;
        //private TRANS2QUIK_REPLY_STRING _transactionReplyBrokerrefById;


        private readonly StringBuilder _errorMessageBuilder = new(1024);

        private GCHandle? _transactionCallbackHandle;

        public EventHandler<TransactionReplyEventArgs>? OnTransactionReply;
        private bool disposedValue;


        public QuikTransactionManager(Trans2QuikAPI api)
        {
            _api = api;

            InitializeDelegates();
            RegisterTransactionReplyCallback();
        }

        private void InitializeDelegates()
        {
            _sendTransaction = GetDelegate<TRANS2QUIK_SEND_SYNC_TRANSACTION>("TRANS2QUIK_SEND_SYNC_TRANSACTION");
            _sendTransactionAsync = GetDelegate<TRANS2QUIK_SEND_ASYNC_TRANSACTION>("TRANS2QUIK_SEND_ASYNC_TRANSACTION");
            _setTransactionReplyCallback = GetDelegate<TRANS2QUIK_SET_TRANSACTIONS_REPLY_CALLBACK>("TRANS2QUIK_SET_TRANSACTIONS_REPLY_CALLBACK");

            _transactionReplyClassCode = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_CLASS_CODE");
            _transactionReplySecCode = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_SEC_CODE");
            _transactionReplyPrice = GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRANSACTION_REPLY_PRICE");
            _transactionReplyQuantity = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRANSACTION_REPLY_QUANTITY");
            _transactionReplyBalance = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRANSACTION_REPLY_BALANCE");
            _transactionReplyFirmid = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_FIRMID");
            _transactionReplyAccount = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_ACCOUNT");
            _transactionReplyClientCode = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_CLIENT_CODE");
            _transactionReplyBrokerref = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_BROKERREF");
            _transactionReplyExchange = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_EXCHANGE_CODE");

            //not implemented in the library
            //_trans2quikTransactionReplyQtyScale = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRANSACTION_REPLY_QTY_SCALE");
            //_transactionReplyOrdersCount = GetDelegate<TRANS2QUIK_REPLY_SHORT>("TRANS2QUIK_TRANSACTION_REPLY_ORDERS_COUNT");
            //_transactionReplyFirstOrderNumberById = GetDelegate<TRANS2QUIK_REPLY_ULONG>("TRANS2QUIK_TRANSACTION_REPLY_FIRST_ORDER_NUMBER_BY_ID");
            //_transactionReplyOrderNumberById = GetDelegate<TRANS2QUIK_REPLY_ULONG>("TRANS2QUIK_TRANSACTION_REPLY_ORDERNUMBER_BY_ID");
            //_transactionReplyPriceById = GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRANSACTION_REPLY_PRICE_BY_ID");
            //_transactionReplyQuantityById = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRANSACTION_REPLY_QUANTITY_BY_ID");
            //_transactionReplyBalanceById = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRANSACTION_REPLY_BALANCE_BY_ID");
            //_transactionReplyFirmidById = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_FIRMID_BY_ID");
            //_transactionReplyAccountById = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_ACCOUNT_BY_ID");
            //_transactionReplyClientCodeById = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_CLIENT_CODE_BY_ID");
            //_transactionReplyBrokerrefById = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRANSACTION_REPLY_BROKERREF_BY_ID");
        }

        private T GetDelegate<T>(string procName) where T : class
        {
            var ptr = _api.GetProcAddress(procName);
            if (ptr == IntPtr.Zero) throw new InvalidOperationException($"PROC not found: {procName}");
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        private void RegisterTransactionReplyCallback()
        {
            var transactionReplyCallback = new TRANS2QUIK_TRANSACTION_REPLY_CALLBACK(TransactionReplyHandler);
            int errorCode = 0;
            _errorMessageBuilder.Clear();
            var result = _setTransactionReplyCallback(transactionReplyCallback, ref errorCode, _errorMessageBuilder, (uint)_errorMessageBuilder.Capacity);
            if (result != Result.SUCCESS)
            {
                throw new QuikTransactionException($"Error setting transaction reply callback: {_errorMessageBuilder.ToString()}", errorCode);
            }
            _transactionCallbackHandle = GCHandle.Alloc(transactionReplyCallback);
        }

        private void TransactionReplyHandler(Result nTransactionResult, int nTransactionExtendedErrorCode, long nTransactionReplyCode, uint dwTransId, ulong nOrderNum, StringBuilder lpcstrTransactionReplyMessage, nint transReplyDescriptor)
        {
            try
            {
                var classCode = _transactionReplyClassCode(transReplyDescriptor);
                var secCode = _transactionReplySecCode(transReplyDescriptor);
                var price = _transactionReplyPrice(transReplyDescriptor);
                var quantity = _transactionReplyQuantity(transReplyDescriptor);
                var balance = _transactionReplyBalance(transReplyDescriptor);
                var firmId = _transactionReplyFirmid(transReplyDescriptor);
                var account = _transactionReplyAccount(transReplyDescriptor);
                var clientCode = _transactionReplyClientCode(transReplyDescriptor);
                var brokerRef = _transactionReplyBrokerref(transReplyDescriptor);
                var exchange = _transactionReplyExchange(transReplyDescriptor);

                //var qtyScale = _trans2quikTransactionReplyQtyScale(transReplyDescriptor);
                //var ordersCount = _transactionReplyOrdersCount(transReplyDescriptor);
                //var firstOrderNumberById = _transactionReplyFirstOrderNumberById(transReplyDescriptor);
                //var orderNumberById = _transactionReplyOrderNumberById(transReplyDescriptor);
                //var priceById = _transactionReplyPriceById(transReplyDescriptor);
                //var quantityById = _transactionReplyQuantityById(transReplyDescriptor);
                //var balanceById = _transactionReplyBalanceById(transReplyDescriptor);
                //var firmIdById = _transactionReplyFirmidById(transReplyDescriptor);
                //var accountIdById = _transactionReplyAccountById(transReplyDescriptor);
                //var clientCodeById = _transactionReplyClientCodeById(transReplyDescriptor);
                //var BrokerrefById = _transactionReplyBrokerrefById(transReplyDescriptor);
            }
            catch (Exception ex)
            {

                throw;
            }


            OnTransactionReply?.Invoke(this, new TransactionReplyEventArgs(nTransactionResult, nTransactionExtendedErrorCode, nTransactionReplyCode, dwTransId, nOrderNum, lpcstrTransactionReplyMessage.ToString()));
        }


        public Trans2QuikTransactionResult SendTransaction(Transaction transaction)
        {
            long errorCode = 0;
            int replyCode = 0;
            uint transactionId = 0;
            ulong orderNum = 0;
            var resultMessage = new StringBuilder(1024);
            _errorMessageBuilder.Clear();

            return new Trans2QuikTransactionResult(
                _sendTransaction(transaction.ToString(),
                                 ref replyCode,
                                 ref transactionId,
                                 ref orderNum,
                                 resultMessage,
                                 (uint)resultMessage.Capacity,
                                 ref errorCode,
                                 _errorMessageBuilder,
                                 (uint)_errorMessageBuilder.Capacity),
                                                   replyCode,
                                                   transactionId,
                                                   orderNum,
                                                   resultMessage.ToString(),
                                                   errorCode,
                                                   _errorMessageBuilder.ToString());
        }

        public Trans2QuikResult SendTranactionAsync(Transaction transaction)
        {
            int errorCode = 0;
            _errorMessageBuilder.Clear();
            return new Trans2QuikResult(
                _sendTransactionAsync(
                    transaction.ToString(),
                    ref errorCode,
                    _errorMessageBuilder,
                    (uint)_errorMessageBuilder.Capacity),
                errorCode,
                _errorMessageBuilder.ToString());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                if (_transactionCallbackHandle.HasValue && _transactionCallbackHandle.Value.IsAllocated)
                {
                    _transactionCallbackHandle?.Free();
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
