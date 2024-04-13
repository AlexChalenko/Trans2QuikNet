namespace Trans2QuikNet.Models
{
    public class Trade
    {

        public int Mode;
        public ulong Number;
        public ulong OrderNumber;
        public string SecCode;
        public string ClassCode;
        public double Price;
        public double Volume1;
        public double Volume2;
        public double Value;

        public Side Side;
        public long Qty;
        public DateTime SettleDate;
        public bool IsMarginal;
        public double AccruedInt;
        public double Yield;
        public double CommClearing;
        public double CommTS;
        public double CommTradingSys;
        public double CommExchange;
        public double CommBroker;
        public double Price2;
        public double AccruedInt2;
        public double RepoRate;
        public double RepoDiscLow;
        public double RepoDiscHigh;
        public double RepoDisc;
        public int RepoTerm;
        public int BlockSec;
        public int Period;
        public int Kind;
        public string Ccy;
        public string SettleCcy;
        public string SettleCode;
        public string Account;
        public string BrokerRef;
        public string ClientCode;
        public string UserId;
        public string FirmId;
        public string PartnerFirmId;
        public string ExchangeCode;
        public string StationId;
        public int TransactionId;


    }
}
