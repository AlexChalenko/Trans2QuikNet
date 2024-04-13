namespace Trans2QuikNet.OrderManager
{
    public class OrderStatusEventArgs : EventArgs
    {
        public int Mode { get; }
        public uint TransactionID { get; }
        public ulong OrderNumber { get; }
        public string ClassCode { get; }
        public string SecurityCode { get; }
        public double Price { get; }
        public long Balance { get; }
        public double Value { get; }
        public bool IsSell { get; }
        public long Status { get; }
        public OrderDetails Details { get; }

        public OrderStatusEventArgs(int mode, uint transID, ulong number, string classCode,
            string secCode, double price, long balance, double value, bool isSell, long status, OrderDetails details)
        {
            Mode = mode;
            TransactionID = transID;
            OrderNumber = number;
            ClassCode = classCode;
            SecurityCode = secCode;
            Price = price;
            Balance = balance;
            Value = value;
            IsSell = isSell;
            Status = status;
            Details = details;
        }

        public override string ToString()
        {
            return $"Mode: {Mode}, TransactionID: {TransactionID}, OrderNumber: {OrderNumber}, ClassCode: {ClassCode}, SecurityCode: {SecurityCode}, Price: {Price}, Balance: {Balance}, Value: {Value}, IsSell: {IsSell}, Status: {Status}, Details: {Details}";
        }
    }
}
