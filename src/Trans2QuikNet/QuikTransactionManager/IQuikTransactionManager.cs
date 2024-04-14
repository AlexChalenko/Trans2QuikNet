using Trans2QuikNet.Models;

namespace Trans2QuikNet
{
    public interface IQuikTransactionManager
    {
        event EventHandler<TransactionReplyEventArgs>? OnTransactionReplyReceived;
        Trans2QuikResult SendTranactionAsync(Transaction transaction);
        Trans2QuikTransactionResult SendTransaction(Transaction transaction);
    }
}