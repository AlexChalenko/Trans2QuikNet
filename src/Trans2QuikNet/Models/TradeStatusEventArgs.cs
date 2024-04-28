namespace Trans2QuikNet.Models
{
    public class TradeStatusEventArgs : EventArgs
    {
        public long Mode { get; }
        public ulong TradeNumber { get; }
        public ulong OrderNumber { get; }
        public string ClassCode { get; }
        public string SecurityCode { get; }
        public double Price { get; }
        public long Quantity { get; }
        public double Value { get; }
        public long IsSell { get; }
        public TradeDetails TradeDetails { get; }

        public TradeStatusEventArgs(int mode, ulong tradeNumber, ulong orderNumber, string classCode,
            string secCode, double price, long quantity, double value, long isSell, TradeDetails tradeDetails)
        {
            Mode = mode;
            TradeNumber = tradeNumber;
            OrderNumber = orderNumber;
            ClassCode = classCode;
            SecurityCode = secCode;
            Price = price;
            Quantity = quantity;
            Value = value;
            IsSell = isSell;
            TradeDetails = tradeDetails;
        }

        public override string ToString()
        {
            return $"Mode: {Mode}, TradeNumber: {TradeNumber}, OrderNumber: {OrderNumber}, ClassCode: {ClassCode}, SecurityCode: {SecurityCode}, Price: {Price}, Quantity: {Quantity}, Value: {Value}, IsSell: {IsSell}, TradeDetails: {TradeDetails}";
        }
    }
}
