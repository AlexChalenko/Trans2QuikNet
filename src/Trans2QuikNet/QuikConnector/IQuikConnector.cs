using Trans2QuikNet.Models;

namespace Trans2QuikNet
{
    public interface IQuikConnector
    {
        /// <summary>
        /// Подключается к терминалу QUIK.
        /// </summary>
        Trans2QuikResult Connect();

        /// <summary>
        /// Отключается от терминала QUIK.
        /// </summary>
        Trans2QuikResult Disconnect();

        /// <summary>
        /// Проверяет, установлено ли подключение к dll.
        /// </summary>
        /// <returns>Возвращает true, если подключение к dll установлено, иначе false.</returns>
        Trans2QuikResult IsDllConnected();

        /// <summary>
        /// Проверяет, подключен ли терминал QUIK к серверу.
        /// </summary>
        /// <returns>Возвращает true, если терминал QUIK подключен к серверу, иначе false.</returns>
        Trans2QuikResult IsQuikConnected();
    }

}
