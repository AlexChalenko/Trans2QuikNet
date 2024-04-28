using Trans2QuikNet.Delegates;
using Trans2QuikNet.Interfaces;
using Trans2QuikNet.Models;
using Trans2QuikNet.Tools;

namespace Trans2QuikNet.Managers
{
    public class QuikOrderManager : IQuikOrderManager, IDisposable
    {
        private readonly Trans2QuikAPI _api;

        private TRANS2QUIK_SUBSCRIBE_ORDERS _subscribeOrders;
        private TRANS2QUIK_START_ORDERS _startOrders;
        private TRANS2QUIK_UNSUBSCRIBE_ORDERS _unsubscribeOrders;
        private TRANS2QUIK_ORDER_STATUS_CALLBACK _orderStatusCallback;


        private TRANS2QUIK_REPLY_LONG _orderQty;
        private TRANS2QUIK_REPLY_LONG _orderDate;
        private TRANS2QUIK_REPLY_LONG _orderTime;
        private TRANS2QUIK_REPLY_LONG _orderActivationTime;
        private TRANS2QUIK_REPLY_LONG _orderWithdrawalTime;
        private TRANS2QUIK_REPLY_LONG _orderExpiry;
        private TRANS2QUIK_REPLY_DOUBLE _orderAccruedInt;
        private TRANS2QUIK_REPLY_DOUBLE _orderYield;
        private TRANS2QUIK_REPLY_STRING _orderUserid;
        private TRANS2QUIK_REPLY_LONG _orderUid;

        private TRANS2QUIK_REPLY_STRING _orderAccount;
        private TRANS2QUIK_REPLY_STRING _orderBrokerRef;
        private TRANS2QUIK_REPLY_STRING _orderClientCode;
        private TRANS2QUIK_REPLY_STRING _orderFirmid;
        private TRANS2QUIK_REPLY_LONG _orderVisibleQty;
        private TRANS2QUIK_REPLY_LONG _orderPeriod;
        private TRANS2QUIK_REPLY_INTPTR _orderFileTime;
        private TRANS2QUIK_REPLY_INTPTR _orderWithdrawFileTime;
        private TRANS2QUIK_ORDER_DATE_TIME _orderDatetime;
        private TRANS2QUIK_REPLY_LONG _orderValueEntryType;

        private TRANS2QUIK_REPLY_LONG _orderExtendedFlags;
        private TRANS2QUIK_REPLY_LONG _orderMinQty;
        private TRANS2QUIK_REPLY_LONG _orderExecType;
        private TRANS2QUIK_REPLY_DOUBLE _orderAvgPrice;
        private TRANS2QUIK_REPLY_STRING _orderRejectionReason;
        private bool disposedValue;

        public event EventHandler<OrderStatusEventArgs> OnOrderStatusReceived;

        public QuikOrderManager(Trans2QuikAPI api)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));

            InitializeDelegates();
        }

        private void InitializeDelegates()
        {
            _subscribeOrders = _api.GetDelegate<TRANS2QUIK_SUBSCRIBE_ORDERS>("TRANS2QUIK_SUBSCRIBE_ORDERS");
            _startOrders = _api.GetDelegate<TRANS2QUIK_START_ORDERS>("TRANS2QUIK_START_ORDERS");
            _unsubscribeOrders = _api.GetDelegate<TRANS2QUIK_UNSUBSCRIBE_ORDERS>("TRANS2QUIK_UNSUBSCRIBE_ORDERS");

            _orderQty = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_QTY");
            _orderDate = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_DATE");
            _orderTime = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_TIME");
            _orderActivationTime = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_ACTIVATION_TIME");
            _orderWithdrawalTime = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_WITHDRAW_TIME");
            _orderExpiry = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_EXPIRY");
            _orderAccruedInt = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_ORDER_ACCRUED_INT");
            _orderYield = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_ORDER_YIELD");
            _orderUserid = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_USERID");
            _orderUid = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_UID");

            _orderAccount = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_ACCOUNT");
            _orderBrokerRef = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_BROKERREF");
            _orderClientCode = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_CLIENT_CODE");
            _orderFirmid = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_FIRMID");
            _orderVisibleQty = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_VISIBLE_QTY");
            _orderPeriod = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_PERIOD");
            _orderFileTime = _api.GetDelegate<TRANS2QUIK_REPLY_INTPTR>("TRANS2QUIK_ORDER_FILETIME");
            _orderWithdrawFileTime = _api.GetDelegate<TRANS2QUIK_REPLY_INTPTR>("TRANS2QUIK_ORDER_WITHDRAW_FILETIME");
            _orderDatetime = _api.GetDelegate<TRANS2QUIK_ORDER_DATE_TIME>("TRANS2QUIK_ORDER_DATE_TIME");
            _orderValueEntryType = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_VALUE_ENTRY_TYPE");

            _orderExtendedFlags = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_EXTENDED_FLAGS");
            _orderMinQty = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_MIN_QTY");
            _orderExecType = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_EXEC_TYPE");
            _orderAvgPrice = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_ORDER_AWG_PRICE");
            _orderRejectionReason = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_REJECT_REASON");
        }

        public Trans2QuikResult SubscribeOrders(string classCode, string secCodes)
        {
            return new Trans2QuikResult(_subscribeOrders(classCode, secCodes), default, string.Empty);
        }

        public Trans2QuikResult StartOrders()
        {
            _orderStatusCallback = new TRANS2QUIK_ORDER_STATUS_CALLBACK(OrderStatusHandler);
            return new Trans2QuikResult(_startOrders(_orderStatusCallback), default, string.Empty);
        }

        public Trans2QuikResult UnsubscribeOrders()
        {
            return new Trans2QuikResult(_unsubscribeOrders(), default, string.Empty);
        }

        private void OrderStatusHandler(int nMode, uint dwTransID, ulong dNumber, string ClassCode, string SecCode, double dPrice, long nBalance, double dValue, long nIsSell, long nStatus, nint orderDescriptor)
        {
            var orderQty = _orderQty(orderDescriptor);
            var orderDate = _orderDate(orderDescriptor);
            var orderTime = _orderTime(orderDescriptor);
            var orderActivationTime = _orderActivationTime(orderDescriptor);
            var orderWithdrawalTime = _orderWithdrawalTime(orderDescriptor);
            var orderExpiry = _orderExpiry(orderDescriptor);
            var orderAccruedInt = _orderAccruedInt(orderDescriptor);
            var orderYield = _orderYield(orderDescriptor);
            var orderUserid = _orderUserid(orderDescriptor);
            var orderUid = _orderUid(orderDescriptor);
            var account = _orderAccount(orderDescriptor);
            var brokerRef = _orderBrokerRef(orderDescriptor);
            var clientCode = _orderClientCode(orderDescriptor);
            var firmid = _orderFirmid(orderDescriptor);
            var visibleQty = _orderVisibleQty(orderDescriptor);
            var period = _orderPeriod(orderDescriptor);
            var dateTime = DateTime.FromFileTime(_orderFileTime(orderDescriptor));
            var withdrawFileTime = DateTime.FromFileTime(_orderWithdrawFileTime(orderDescriptor));
            var orderDatetime = FromDescriptor(orderDescriptor);
            var cancelDatetime = FromDescriptor(orderDescriptor);
            var valueEntryType = _orderValueEntryType(orderDescriptor);
            var extendedFlags = _orderExtendedFlags(orderDescriptor);
            var minQty = _orderMinQty(orderDescriptor);
            var execType = _orderExecType(orderDescriptor);
            var avgPrice = _orderAvgPrice(orderDescriptor);
            var rejectionReason = _orderRejectionReason(orderDescriptor);

            var newOrderDetails = new OrderDetails()
            {
                UserId = orderUserid,
                Account = account,
                BrokerRef = brokerRef,
                ClientCode = clientCode,
                FirmId = firmid,
                RejectReason = rejectionReason,
                Qty = orderQty,
                OrderDate = orderDate,
                OrderTime = orderTime,
                OrderActivationTime = orderActivationTime,
                OrderWithdrawalTime = orderWithdrawalTime,
                OrderExpiry = orderExpiry,
                OrderAccruedInt = orderAccruedInt,
                OrderYield = orderYield,
                OrderUid = orderUid,
                VisibleQty = visibleQty,
                Period = period,
                OrderDatetime = orderDatetime,
                CancelDatetime = cancelDatetime,
                ValueEntryType = valueEntryType,
                ExtendedFlags = extendedFlags,
                MinQty = minQty,
                ExecType = execType,
                AvgPrice = avgPrice,
                Datetime = dateTime,
                WithdrawDatetime = withdrawFileTime
            };

            OnOrderStatusReceived?.Invoke(this,
                new OrderStatusEventArgs(
                    nMode,
                    dwTransID,
                    dNumber,
                    ClassCode,
                    SecCode,
                    dPrice,
                    nBalance,
                    dValue,
                    nIsSell == 1,
                    nStatus,
                    newOrderDetails));
        }

        private DateTime FromDescriptor(nint datetimeDescriptor)
        {
            if (_orderDatetime(datetimeDescriptor, 0) > 0)
            {
                var date = DateTime.ParseExact(_orderDatetime(datetimeDescriptor, 0).ToString(), "yyyyMMdd", null);
                var time = DateTime.ParseExact(_orderDatetime(datetimeDescriptor, 1).ToString(), "HHmmss", null);
                var microseconds = double.Parse(_orderDatetime(datetimeDescriptor, 2).ToString());
                var orderDatetime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
                orderDatetime.AddMicroseconds(microseconds);
                return orderDatetime;
            }
            else
            {
                return default;
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

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~QuikOrderManager()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
