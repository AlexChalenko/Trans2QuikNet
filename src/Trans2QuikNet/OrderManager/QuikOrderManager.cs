using System.Runtime.InteropServices;
using Trans2QuikNet.Delegates;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.OrderManager
{
    public class QuikOrderManager
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

        public event EventHandler<OrderStatusEventArgs> OnOrderStatusReceived;

        public QuikOrderManager(Trans2QuikAPI api)
        {
            _api = api;

            InitializeDelegates();

            //    _orderStatusCallback = new TRANS2QUIK_ORDER_STATUS_CALLBACK(OrderStatusCallback);
        }

        private void InitializeDelegates()
        {
            _subscribeOrders = GetDelegate<TRANS2QUIK_SUBSCRIBE_ORDERS>("TRANS2QUIK_SUBSCRIBE_ORDERS");
            _startOrders = GetDelegate<TRANS2QUIK_START_ORDERS>("TRANS2QUIK_START_ORDERS");
            _unsubscribeOrders = GetDelegate<TRANS2QUIK_UNSUBSCRIBE_ORDERS>("TRANS2QUIK_UNSUBSCRIBE_ORDERS");

            _orderQty = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_QTY");
            _orderDate = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_DATE");
            _orderTime = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_TIME");
            _orderActivationTime = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_ACTIVATION_TIME");
            _orderWithdrawalTime = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_WITHDRAW_TIME");
            _orderExpiry = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_EXPIRY");
            _orderAccruedInt = GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_ORDER_ACCRUED_INT");
            _orderYield = GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_ORDER_YIELD");
            _orderUserid = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_USERID");
            _orderUid = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_UID");

            _orderAccount = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_ACCOUNT");
            _orderBrokerRef = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_BROKERREF");
            _orderClientCode = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_CLIENT_CODE");
            _orderFirmid = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_FIRMID");
            _orderVisibleQty = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_VISIBLE_QTY");
            _orderPeriod = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_PERIOD");
            _orderFileTime = GetDelegate<TRANS2QUIK_REPLY_INTPTR>("TRANS2QUIK_ORDER_FILETIME");
            _orderWithdrawFileTime = GetDelegate<TRANS2QUIK_REPLY_INTPTR>("TRANS2QUIK_ORDER_WITHDRAW_FILETIME");
            _orderDatetime = GetDelegate<TRANS2QUIK_ORDER_DATE_TIME>("TRANS2QUIK_ORDER_DATE_TIME");
            _orderValueEntryType = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_VALUE_ENTRY_TYPE");

            _orderExtendedFlags = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_EXTENDED_FLAGS");
            _orderMinQty = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_MIN_QTY");
            _orderExecType = GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_ORDER_EXEC_TYPE");
            _orderAvgPrice = GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_ORDER_AWG_PRICE");
            _orderRejectionReason = GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_ORDER_REJECT_REASON");
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

        private T GetDelegate<T>(string procName) where T : class
        {
            var ptr = _api.GetProcAddress(procName);
            if (ptr == IntPtr.Zero) throw new InvalidOperationException($"PROC not found: {procName}");
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
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
    }
}
