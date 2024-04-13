namespace Trans2QuikNet.Models
{
    public class Order
    {

        public int TransactionId;
        public Side Side;
        public ulong OrderNum;
        public double Qty;
        public DateTime Date;
        public DateTime? ActivationDate;
        public DateTime? WithdrawalDate;
        public DateTime? ExpiryDate;
        public double AccruedInt;
        public double Yield;
        public int UID;
        public double VisibleQty;
        public int Period;
        public int ExtendedFlags;
        public double MinQty;
        public int ExecType;
        public double AwgPrice;
        public string UserId;
        public string Account;
        public string BrokerRef;
        public string ClientCode;
        public string RejectReason;
        public string ClassCode;
        public string SecCode;
        public double Price;
        public double Balance;
        public double Value;
        public int Status;
    }
}
