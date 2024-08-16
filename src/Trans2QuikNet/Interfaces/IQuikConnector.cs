using Trans2QuikNet.Models;

namespace Trans2QuikNet.Interfaces
{
    public interface IQuikConnector
    {
        event EventHandler<ConnectionStatusEventArgs>? OnConnectionStatusChanged;

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
        /// <returns>
        /// <list type="bullet">
        /// <item>
        /// TRANS2QUIK_DLL_CONNECTED – соединение библиотеки Trans2QUIK.dll с терминалом QUIK установлено;
        /// </item>
        /// <item>
        /// TRANS2QUIK_DLL_NOT_CONNECTED – не установлена связь библиотеки Trans2QUIK.dll с терминалом QUIK
        /// </item>
        /// </list>
        /// </returns>
        Trans2QuikResult IsDllConnected();

        /// <summary>
        /// Проверяет, подключен ли терминал QUIK к серверу.
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        /// <item>
        /// TRANS2QUIK_QUIK_CONNECTED – соединение установлено;
        /// </item>
        /// <item>
        /// TRANS2QUIK_QUIK_NOT_CONNECTED – соединение не установлено;
        /// </item>
        /// <item>
        /// TRANS2QUIK_DLL_NOT_CONNECTED – не установлена связь библиотеки Trans2QUIK.dll с терминалом QUIK. В этом случае проверить наличие или отсутствие связи терминала QUIK с сервером невозможно
        /// </item>
        /// </list>
        /// </returns>
        Trans2QuikResult IsQuikConnected();
    }

}
