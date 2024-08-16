using Trans2QuikNet.Models;

namespace Trans2QuikNet.Interfaces;

public interface IQuikTradeManager
{
    event EventHandler<TradeStatusEventArgs> OnTradeStatusReceived;
    /// <summary>
    /// Функция служит для создания списка классов и инструментов для подписки на получение сделок по ним.
    /// </summary>
    /// <param name="classCode">Код класса, для которого будут заказаны сделки, если в качестве обоих входных параметров указаны пустые строки, то это означает, что заказано получение сделок по всем доступным инструментам</param>
    /// <param name="secCodes">Список кодов инструментов, разделенных символом «|», по которым будут заказаны сделки. Если в качестве значения указана пустая строка, то это означает, что заказано получение сделок по классу, указанному в параметре lpstrClassCode </param>
    /// <returns></returns>
    Trans2QuikResult SubscribeTrades(string classCode, string secCodes);
    Trans2QuikResult UnsubscribeTrades();
    Trans2QuikResult StartTrades();
}