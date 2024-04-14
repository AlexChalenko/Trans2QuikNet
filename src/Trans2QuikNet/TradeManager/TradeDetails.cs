namespace Trans2QuikNet.TradeManager
{
    public class TradeDetails
    {
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

        public long TradeDate { get; set; }
        public long TradeSettleDate { get; set; }
        public long TradeTime { get; set; }
        public long TradeIsMarginal { get; set; }
        public double AccruedInterest { get; set; }
        public double Yield { get; set; }
        public double TSCommission { get; set; }
        public double ClearingCenterCommission { get; set; }
        public double ExchangeCommission { get; set; }
        public double TradingSystemCommission { get; set; }

        public double Price2 { get; set; }
        public double RepoRate { get; set; }
        public double RepoValue { get; set; }
        public double Repo2Value { get; set; }
        public double AccruedInterest2 { get; set; }
        public long RepoTerm { get; set; }
        public double StartDiscount { get; set; }
        public double LowerDiscount { get; set; }
        public double UpperDiscount { get; set; }
        public long BlockSecurities { get; set; }

        public long Period { get; set; }
        public long Kind { get; set; }
        public DateTime FileTime {get; set;}
        public DateTime DateTime { get; set; }
        public double BrokerCommission { get; set; }
        public long TransId { get; set; }
        public long QtyScale { get; set; }
        public string Currency { get; set; }
        public string SettleCurrency { get; set; }
        public string SettleCode { get; set; }
        
        public string Account { get; set; }
        public string BrokerRef { get; set; }
        public string ClientCode { get; set; }
        public string UserId { get; set; }
        public string FirmId { get; set; }
        public string PartnerFirmId { get; set; }
        public string ExchangeCode { get; set; }
        public string StationId { get; set; }

        public override string ToString()
        {
            return $"TradeDate: {TradeDate}, TradeSettleDate: {TradeSettleDate}, TradeTime: {TradeTime}, TradeIsMarginal: {TradeIsMarginal}, AccruedInterest: {AccruedInterest}, Yield: {Yield}, TSCommission: {TSCommission}, ClearingCenterCommission: {ClearingCenterCommission}, ExchangeCommission: {ExchangeCommission}, TradingSystemCommission: {TradingSystemCommission}, Price2: {Price2}, RepoRate: {RepoRate}, RepoValue: {RepoValue}, RepoValue2: {Repo2Value}, AccruedInterest2: {AccruedInterest2}, RepoTerm: {RepoTerm}, StartDiscount: {StartDiscount}, LowerDiscount: {LowerDiscount}, UpperDiscount: {UpperDiscount}, BlockSecurities: {BlockSecurities}, Period: {Period}, Kind: {Kind}, FileTime: {FileTime}, DateTime: {DateTime}, BrokerCommission: {BrokerCommission}, TransId: {TransId}, QtyScale: {QtyScale}, Currency: {Currency}, SettleCurrency: {SettleCurrency}, SettleCode: {SettleCode}, Account: {Account}, BrokerRef: {BrokerRef}, ClientCode: {ClientCode}, UserId: {UserId}, FirmId: {FirmId}, PartnerFirmId: {PartnerFirmId}, ExchangeCode: {ExchangeCode}, StationId: {StationId}";
        }

    }
}