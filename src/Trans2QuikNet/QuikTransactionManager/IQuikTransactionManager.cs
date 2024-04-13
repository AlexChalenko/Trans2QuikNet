using Trans2QuikNet.Models;

namespace Trans2QuikNet
{
    public interface IQuikTransactionManager
    {
        void Dispose();
        Trans2QuikResult SendTranactionAsync(Transaction transaction);
        Trans2QuikTransactionResult SendTransaction(Transaction transaction);
    }
}