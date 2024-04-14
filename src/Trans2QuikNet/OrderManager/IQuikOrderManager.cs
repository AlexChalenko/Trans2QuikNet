using Trans2QuikNet.Models;

namespace Trans2QuikNet.OrderManager
{
    public interface IQuikOrderManager
    {
        event EventHandler<OrderStatusEventArgs> OnOrderStatusReceived;
        Trans2QuikResult StartOrders();
        Trans2QuikResult SubscribeOrders(string classCode, string secCodes);
        Trans2QuikResult UnsubscribeOrders();
    }
}