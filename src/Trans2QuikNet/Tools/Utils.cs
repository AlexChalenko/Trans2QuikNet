using System.Text;
using System.Text.RegularExpressions;
using Trans2QuikNet.Models;

namespace Trans2QuikNet.Tools
{
    public static partial class Utils
    {
        private readonly static Encoding RU = Encoding.GetEncoding(1251);

        public static string Str(byte[] arr)
        {
            if (arr == null)
            {
                return null;
            }
            return RU.GetString(arr).TrimEnd('\0');// Для чтения C строк
        }

        private static volatile int i = 1;
        public static int GetNextTrnId()
        {
            if (i > 2147483640) i = 1; //Лимит Trans2Quik.dll - 2147483646
            return i++;
        }

        public static DateTime? FromArqaDate(long Date)
        {
            if (Date < 1) return null;
            return DateTime.ParseExact(Date.ToString(), "yyyyMMdd", null);
        }

        public static DateTime? FromArqaTime(int Time)
        {
            if (Time < 1) return null;
            return DateTime.ParseExact(Time.ToString(), "HHmmss", null);
        }

        public static DateTime FromArqaDealDate(long Date, long Time, long MCS)
        {
            Match match = DateRegex().Match(Time.ToString());
            if (match.Success)
            {
                int hours = int.Parse(match.Groups[1].Value);
                int minutes = int.Parse(match.Groups[2].Value);
                int seconds = int.Parse(match.Groups[3].Value);
                TimeSpan timeSpan = new(hours, minutes, seconds);
                return FromArqaDate(Date).Value
                    .Add(timeSpan)
                    .Add(TimeSpan.ParseExact(MCS.ToString("000000"), "ffffff", null));
            }
            return default;
        }

        public static void AppendFieldAndValue(this StringBuilder sb, string Field, object Value)
        {
            switch (Value)
            {
                case null: return;
                case int i when i == 0: return;
                case uint i when i == 0U: return;
                case long i when i == 0L: return;
                case ulong i when i == 0UL: return;
                case double i when i == 0d: return;
                case decimal i when i == 0m: return;
            }
            sb.Append(Field);
            sb.Append("=");
            sb.Append(Value.ToString());
            sb.Append("; ");
        }

        public static string ErrorDecode(Result Result)
        {
            var errorDict = new Dictionary<Result, string>()
            {
                { Result.SUCCESS, "Успешно" },
                { Result.FAILED, "Ошибка" },
                { Result.QUIK_TERMINAL_NOT_FOUND, "Терминал Quik не найден" },
                { Result.DLL_VERSION_NOT_SUPPORTED, "Терминал Quik не поддерживает эту версию Trans2Quik" },
                { Result.ALREADY_CONNECTED_TO_QUIK, "Уже подключен к Quik" },
                { Result.WRONG_SYNTAX, "Ошибка синтаксиса" },
                { Result.QUIK_NOT_CONNECTED, "Терминал не подключен к серверу Quik" },
                { Result.DLL_NOT_CONNECTED, "Нет подключения к терминалу Quik" },
                { Result.QUIK_CONNECTED, "Терминал подключён к серверу Quik" },
                { Result.QUIK_DISCONNECTED, "Терминал отключён от сервера Quik" },
                { Result.DLL_CONNECTED, "Есть подключение к терминалу Quik" },
                { Result.DLL_DISCONNECTED, "Нет подключение к терминалу Quik" },
                { Result.MEMORY_ALLOCATION_ERROR, "Ошибка выделения памяти Trans2Quik" },
                { Result.WRONG_CONNECTION_HANDLE, "Неверный хэндл Trans2Quik" },
                { Result.WRONG_INPUT_PARAMS, "Ошибка параметров Trans2Quik" }
            };

            return errorDict.TryGetValue(Result, out string? decoded) ? decoded : "Неизвестная ошибка";
        }

        public static string DecodeTransactionReply(int replyCode)
        {
            var replyDict = new Dictionary<int, string>()
            {
                {0, "транзакция отправлена серверу"},
                {1, "транзакция получена на сервер QUIK от клиента"},
                {2, "ошибка при передаче транзакции в торговую систему, поскольку отсутствует подключение шлюза Московской Биржи, повторно транзакция не отправляется"},
                {3, "транзакция выполнена"},
                {4, "транзакция не выполнена торговой системой, код ошибки торговой системы будет указан в поле «DESCRIPTION»"},
                {5, "транзакция не прошла проверку сервера QUIK по каким - либо критериям.Например, проверку на наличие прав у пользователя на отправку транзакции данного типа "},
                {6, "транзакция не прошла проверку лимитов сервера QUIK "},
                {10, "транзакция не поддерживается торговой системой. К примеру, попытка отправить «ACTION = MOVE_ORDERS» на Московской Бирже "},
                {11, "транзакция не прошла проверку правильности электронной подписи. К примеру, если ключи, зарегистрированные на сервере, не соответствуют подписи отправленной транзакции"},
                {12, "не удалось дождаться ответа на транзакцию, т.к.истек таймаут ожидания.Может возникнуть при подаче транзакций из QPILE"},
                {13, "транзакция отвергнута, т.к.ее выполнение могло привести к кросс-сделке(т.е.сделке с тем же самым клиентским счетом)"},
                {14, "транзакция не прошла контроль дополнительных ограничений"},
                {15, "транзакция принята после нарушения дополнительных ограничений"},
                {16, "транзакция отменена пользователем в ходе проверки дополнительных ограничений"}
            };

            return replyDict.TryGetValue(replyCode, out string decoded) ? decoded : "Неизвестная ошибка";
        }

        private static readonly Dictionary<long, string> nModeDict = new Dictionary<long, string>()
        {
            {0, "Новая заявка"},
            {1, "Идет начальное получание заявок" },
            {2, "Получена последняя заявка из начальной рассылки"},
        };

        public static string DecodeOrderNMode(long nMode)
        {
            return nModeDict.TryGetValue(nMode, out string? decoded) ? decoded : "Неизвестный признак получение заявок";
        }

        private static Dictionary<long, string> nOrderStatusDict = new Dictionary<long, string>()
        {
            {0, "Исполнена"},
            {1, "Активна"},
            {2, "Снята"},
        };

        public static string DecodeNStatus(long nStatus)
        {
            return nOrderStatusDict.TryGetValue(nStatus, out string? decoded) ? decoded : "Неизвестное состояние исполнения заявки";
        }

        private static readonly Dictionary<long, string> nTradeStatusDict = new Dictionary<long, string>()
        {
            {1, "Обычная" },
            {2, "Адресная" },
            {3, "Первичное размещение" },
            {4, "Перевод денег / инструментов" },
            {5, "Адресная сделка первой части РЕПО" },
            {6, "Расчетная по операции своп" },
            {7, "Расчетная по внебиржевой операции своп" },
            {8, "Расчетная сделка бивалютной корзины" },
            {9, "Расчетная внебиржевая сделка бивалютной корзины" },
        };

        public static string DecodeTradeStatus(long nTradeStatus)
        {
            return nTradeStatusDict.TryGetValue(nTradeStatus, out string? decoded) ? decoded : "Неизвестный вид сделки";
        }

        [GeneratedRegex(@"(\d)(\d{2})(\d{2})")]
        private static partial Regex DateRegex();
    }
}
