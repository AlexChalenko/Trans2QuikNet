using Trans2QuikNet.Delegates;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.TradeManager
{
    public class QuikTradeManager : IQuikTradeManager, IDisposable
    {
        private readonly Trans2QuikAPI _api;
        private TRANS2QUIK_START_TRADES _startTrades;
        private TRANS2QUIK_SUBSCRIBE_TRADES _subscribeTrades;
        private TRANS2QUIK_UNSUBSCRIBE_TRADES _unsubscribeTrades;
        private TRANS2QUIK_TRADE_STATUS_CALLBACK _tradeStatusCallback;

        /*
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_DATE (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_SETTLE_DATE (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_TIME (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_IS_MARGINAL (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_ACCRUED_INT (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_YIELD (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_TS_COMMISSION (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_CLEARING_CENTER_COMMISSION (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_EXCHANGE_COMMISSION (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_TRADING_SYSTEM_COMMISSION (TradeDescriptor tradeDescriptor);

double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_PRICE2 (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_REPO_RATE (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_REPO_VALUE (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_REPO2_VALUE (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_ACCRUED_INT2 (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_REPO_TERM (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_START_DISCOUNT (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_LOWER_DISCOUNT (TradeDescriptor tradeDescriptor);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_UPPER_DISCOUNT (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_BLOCK_SECURITIES (TradeDescriptor tradeDescriptor);

long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_PERIOD (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_KIND (TradeDescriptor tradeDescriptor);
FILETIME TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_FILETIME (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_DATE_TIME (TradeDescriptor tradeDescriptor, long nTimeType);
double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_BROKER_COMMISSION (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_TRANSID (TradeDescriptor tradeDescriptor);
long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_QTY_SCALE (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_CURRENCY (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_SETTLE_CURRENCY (TradeDescriptor tradeDescriptor);

LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_SETTLE_CODE (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_ACCOUNT (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_BROKERREF (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_CLIENT_CODE (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_USERID (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_FIRMID (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_PARTNER_FIRMID (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_EXCHANGE_CODE (TradeDescriptor tradeDescriptor);
LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_STATION_ID (TradeDescriptor tradeDescriptor);
        */
        private TRANS2QUIK_REPLY_LONG _tradeDate;
        private TRANS2QUIK_REPLY_LONG _tradeSettleDate;
        private TRANS2QUIK_REPLY_LONG _tradeTime;
        private TRANS2QUIK_REPLY_LONG _tradeMarginal;
        private TRANS2QUIK_REPLY_DOUBLE _tradeAccruedInt;
        private TRANS2QUIK_REPLY_DOUBLE _tradeYield;
        private TRANS2QUIK_REPLY_DOUBLE _tradeTsCommission;
        private TRANS2QUIK_REPLY_DOUBLE _tradeClearingCenterCommission;
        private TRANS2QUIK_REPLY_DOUBLE _tradeExchangeCommission;
        private TRANS2QUIK_REPLY_DOUBLE _tradeTradingSystemCommission;

        private TRANS2QUIK_REPLY_DOUBLE _tradePrice2;
        private TRANS2QUIK_REPLY_DOUBLE _tradeRepoRate;
        private TRANS2QUIK_REPLY_DOUBLE _tradeRepoValue;
        private TRANS2QUIK_REPLY_DOUBLE _tradeRepo2Value;
        private TRANS2QUIK_REPLY_DOUBLE _tradeAccruedInt2;
        private TRANS2QUIK_REPLY_LONG _tradeRepoTerm;
        private TRANS2QUIK_REPLY_DOUBLE _tradeStartDiscount;
        private TRANS2QUIK_REPLY_DOUBLE _tradeLowerDiscount;
        private TRANS2QUIK_REPLY_DOUBLE _tradeUpperDiscount;
        private TRANS2QUIK_REPLY_LONG _tradeBlockSecurities;

        private TRANS2QUIK_REPLY_LONG _tradePeriod;
        private TRANS2QUIK_REPLY_LONG _tradeKind;
        private TRANS2QUIK_REPLY_INTPTR _tradeFileTime;
        private TRANS2QUIK_TRADE_DATE_TIME _tradeDateTime;
        private TRANS2QUIK_REPLY_DOUBLE _tradeBrokerCommission;
        private TRANS2QUIK_REPLY_LONG _tradeTransId;
        //private TRANS2QUIK_REPLY_LONG _tradeQtyScale;
        private TRANS2QUIK_REPLY_STRING _tradeCurrency;
        private TRANS2QUIK_REPLY_STRING _tradeSettleCurrency;
        private TRANS2QUIK_REPLY_STRING _tradeSettleCode;

        private TRANS2QUIK_REPLY_STRING _tradeAccount;
        private TRANS2QUIK_REPLY_STRING _tradeBrokerRef;
        private TRANS2QUIK_REPLY_STRING _tradeClientCode;
        private TRANS2QUIK_REPLY_STRING _tradeUserId;
        private TRANS2QUIK_REPLY_STRING _tradeFirmId;
        private TRANS2QUIK_REPLY_STRING _tradePartnerFirmId;
        private TRANS2QUIK_REPLY_STRING _tradeExchangeCode;
        private TRANS2QUIK_REPLY_STRING _tradeStationId;
        private bool disposedValue;

        public event EventHandler<TradeStatusEventArgs> OnTradeStatusReceived;

        public QuikTradeManager(Trans2QuikAPI api)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            InitializeDelegates();
        }

        private void InitializeDelegates()
        {
            _subscribeTrades = _api.GetDelegate<TRANS2QUIK_SUBSCRIBE_TRADES>("TRANS2QUIK_SUBSCRIBE_TRADES");
            _unsubscribeTrades = _api.GetDelegate<TRANS2QUIK_UNSUBSCRIBE_TRADES>("TRANS2QUIK_UNSUBSCRIBE_TRADES");
            _startTrades = _api.GetDelegate<TRANS2QUIK_START_TRADES>("TRANS2QUIK_START_TRADES");
            _tradeStatusCallback = new TRANS2QUIK_TRADE_STATUS_CALLBACK(TradeStatusHandler);

            _tradeDate = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_DATE");
            _tradeSettleDate = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_SETTLE_DATE");
            _tradeTime = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_TIME");
            _tradeMarginal = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_IS_MARGINAL");
            _tradeAccruedInt = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_ACCRUED_INT");
            _tradeYield = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_YIELD");
            _tradeTsCommission = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_TS_COMMISSION");
            _tradeClearingCenterCommission = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_CLEARING_CENTER_COMMISSION");
            _tradeExchangeCommission = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_EXCHANGE_COMMISSION");
            _tradeTradingSystemCommission = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_TRADING_SYSTEM_COMMISSION");

            _tradePrice2 = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_PRICE2");
            _tradeRepoRate = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_REPO_RATE");
            _tradeRepoValue = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_REPO_VALUE");
            _tradeRepo2Value = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_REPO2_VALUE");
            _tradeAccruedInt2  = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_ACCRUED_INT2");
            _tradeRepoTerm = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_REPO_TERM");
            _tradeStartDiscount = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_START_DISCOUNT"); 
            _tradeLowerDiscount = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_LOWER_DISCOUNT");
            _tradeUpperDiscount = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_UPPER_DISCOUNT");
            _tradeBlockSecurities = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_BLOCK_SECURITIES");

            _tradePeriod = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_PERIOD");
            _tradeKind = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_KIND");
            _tradeFileTime = _api.GetDelegate<TRANS2QUIK_REPLY_INTPTR>("TRANS2QUIK_TRADE_FILETIME");
            _tradeDateTime = _api.GetDelegate<TRANS2QUIK_TRADE_DATE_TIME>("TRANS2QUIK_TRADE_DATE_TIME");
            _tradeBrokerCommission = _api.GetDelegate<TRANS2QUIK_REPLY_DOUBLE>("TRANS2QUIK_TRADE_BROKER_COMMISSION");
            _tradeTransId = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_TRANSID");
            //_tradeQtyScale = _api.GetDelegate<TRANS2QUIK_REPLY_LONG>("TRANS2QUIK_TRADE_QTY_SCALE");
            _tradeCurrency = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_CURRENCY");
            _tradeSettleCurrency = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_SETTLE_CURRENCY");
            _tradeSettleCode = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_SETTLE_CODE");

            _tradeAccount = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_ACCOUNT");
            _tradeBrokerRef = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_BROKERREF");
            _tradeClientCode = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_CLIENT_CODE");
            _tradeUserId = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_USERID");
            _tradeFirmId = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_FIRMID");
            _tradePartnerFirmId = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_PARTNER_FIRMID");
            _tradeExchangeCode = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_EXCHANGE_CODE");
            _tradeStationId = _api.GetDelegate<TRANS2QUIK_REPLY_STRING>("TRANS2QUIK_TRADE_STATION_ID");
        }

        public Trans2QuikResult SubscribeTrades(string classCode, string secCodes)
        {
            return new Trans2QuikResult(_subscribeTrades(classCode, secCodes), default, string.Empty);
        }

        public Trans2QuikResult UnsubscribeTrades()
        {
            return new Trans2QuikResult(_unsubscribeTrades(), default, string.Empty);
        }

        public Trans2QuikResult StartTrades()
        {
            return new Trans2QuikResult(_startTrades(_tradeStatusCallback), default, string.Empty);
        }

        private void TradeStatusHandler(long nMode, ulong dNumber, ulong nOrderNumber, string ClassCode, string SecCode, double dPrice, long nQty, double dValue, long nIsSell, nint tradeDescriptor)
        {
            var tradeDetails = new TradeDetails()
            {
                TradeDate = _tradeDate(tradeDescriptor),
                TradeSettleDate = _tradeSettleDate(tradeDescriptor),
                TradeTime = _tradeTime(tradeDescriptor),
                TradeIsMarginal = _tradeMarginal(tradeDescriptor),
                AccruedInterest = _tradeAccruedInt(tradeDescriptor),
                Yield = _tradeYield(tradeDescriptor),
                TSCommission = _tradeTsCommission(tradeDescriptor),
                ClearingCenterCommission = _tradeClearingCenterCommission(tradeDescriptor),
                ExchangeCommission = _tradeExchangeCommission(tradeDescriptor),
                TradingSystemCommission = _tradeTradingSystemCommission(tradeDescriptor),

                Price2 = _tradePrice2(tradeDescriptor),
                RepoRate = _tradeRepoRate(tradeDescriptor),
                RepoValue = _tradeRepoValue(tradeDescriptor),
                Repo2Value = _tradeRepo2Value(tradeDescriptor),
                AccruedInterest2 = _tradeAccruedInt2(tradeDescriptor),
                RepoTerm = _tradeRepoTerm(tradeDescriptor),
                StartDiscount = _tradeStartDiscount(tradeDescriptor),
                LowerDiscount = _tradeLowerDiscount(tradeDescriptor),
                UpperDiscount = _tradeUpperDiscount(tradeDescriptor),
                BlockSecurities = _tradeBlockSecurities(tradeDescriptor),

                Period = _tradePeriod(tradeDescriptor),
                Kind = _tradeKind(tradeDescriptor),
                FileTime = new DateTime(_tradeFileTime(tradeDescriptor).ToInt64()),
                DateTime = FromDescriptor(tradeDescriptor),
                BrokerCommission = _tradeBrokerCommission(tradeDescriptor),
                TransId = _tradeTransId(tradeDescriptor),
                //QtyScale = _tradeQtyScale(tradeDescriptor),
                Currency = _tradeCurrency(tradeDescriptor),
                SettleCurrency = _tradeSettleCurrency(tradeDescriptor),
                SettleCode = _tradeSettleCode(tradeDescriptor),

                Account = _tradeAccount(tradeDescriptor),
                BrokerRef = _tradeBrokerRef(tradeDescriptor),
                ClientCode = _tradeClientCode(tradeDescriptor),
                UserId = _tradeUserId(tradeDescriptor), 
                FirmId = _tradeFirmId(tradeDescriptor),
                PartnerFirmId = _tradePartnerFirmId(tradeDescriptor),
                ExchangeCode = _tradeExchangeCode(tradeDescriptor),
                StationId = _tradeStationId(tradeDescriptor)
            };
            
            OnTradeStatusReceived?.Invoke(this, new TradeStatusEventArgs((int)nMode, dNumber, nOrderNumber, ClassCode, SecCode, dPrice, nQty, dValue, nIsSell, tradeDetails));
        }

        private DateTime FromDescriptor(nint datetimeDescriptor)
        {
            if (_tradeDateTime(datetimeDescriptor, 0) > 0)
            {
                var date = DateTime.ParseExact(_tradeDateTime(datetimeDescriptor, 0).ToString(), "yyyyMMdd", null);
                var time = DateTime.ParseExact(_tradeDateTime(datetimeDescriptor, 1).ToString(), "HHmmss", null);
                var microseconds = double.Parse(_tradeDateTime(datetimeDescriptor, 2).ToString());
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
        // ~QuikTradeManager()
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
