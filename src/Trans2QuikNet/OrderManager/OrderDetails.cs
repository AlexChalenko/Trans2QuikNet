
namespace Trans2QuikNet.OrderManager
{
    public class OrderDetails
    {
        public string UserId { get; set; }
        public string Account { get; set; }
        public string BrokerRef { get; set; }
        public string ClientCode { get; set; }
        public string FirmId { get; set; }
        public string RejectReason { get; set; }
        public long Qty { get; set; }
        public long OrderDate { get; set; }
        public long OrderTime { get; set; }
        public long OrderActivationTime { get; set; }
        public long OrderWithdrawalTime { get; set; }
        public long OrderExpiry { get; set; }
        public double OrderAccruedInt { get; set; }
        public double OrderYield { get; set; }
        public long OrderUid { get; set; }
        public long VisibleQty { get; set; }
        public long Period { get; set; }
        public DateTime OrderDatetime { get; set; }
        public DateTime CancelDatetime { get; set; }
        public long ValueEntryType { get; set; }
        public long ExtendedFlags { get; set; }
        public long MinQty { get; set; }
        public long ExecType { get; set; }
        public double AvgPrice { get; set; }
        public DateTime Datetime { get; set; }
        public DateTime WithdrawDatetime { get; set; }

        public override string ToString()
        {
            return $"\nUserId: {UserId}, Account: {Account}, BrokerRef: {BrokerRef}, ClientCode: {ClientCode}, FirmId: {FirmId}, RejectReason: {RejectReason}, Qty: {Qty}, OrderDate: {OrderDate}, OrderTime: {OrderTime}, OrderActivationTime: {OrderActivationTime}, OrderWithdrawalTime: {OrderWithdrawalTime}, OrderExpiry: {OrderExpiry}, OrderAccruedInt: {OrderAccruedInt}, OrderYield: {OrderYield}, OrderUid: {OrderUid}, VisibleQty: {VisibleQty}, Period: {Period}, OrderDatetime: {OrderDatetime}, ValueEntryType: {ValueEntryType}, ExtendedFlags: {ExtendedFlags}, MinQty: {MinQty}, ExecType: {ExecType}, AvgPrice: {AvgPrice}";
        }
    }
}
