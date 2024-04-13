using System.Text;
using Trans2QuikNet.Tools;

namespace Trans2QuikNet.Models
{
    public class Transaction
    {

        public Transaction() => InternalId = Utils.GetNextTrnId();

        public Transaction(int TrnId) => InternalId = TrnId;

        public ExecCondiotion ExecutionCondition;

        /// <summary>
        /// Внутренний ID транзакции
        /// </summary>
        public readonly int InternalId;

        /// <summary>
        /// Тип транзакции (Новый ордер, отмена, изменение)
        /// </summary>
        public TranType TransactionType = TranType.New;

        /// <summary>
        /// Номер ордера в торговой системе. Заполняется по результатм транзакции
        /// </summary>
        public ulong OrderNum;

        /// <summary>
        /// Код класса, например TQBR
        /// </summary>
        public string ClassCode;

        /// <summary>
        /// Код ценной бумаги, например HYDR
        /// </summary>
        public string SecCode;

        /// <summary>
        /// Направление сделки, покупка или продажа
        /// </summary>
        public Side Side;

        /// <summary>
        /// Цена заявки
        /// </summary>
        public double Price;

        /// <summary>
        /// Счёт заявки. Брокерский обычно "L01-00000F00"
        /// </summary>
        public string Account;

        /// <summary>
        /// Количество
        /// </summary>
        public double Qty;

        /// <summary>
        /// Код клиента в Quik
        /// </summary>
        public string ClientCode;

        /// <summary>
        /// Коментарий к ордеры
        /// </summary>
        public string Comment;

        /// <summary>
        /// Сообщение терминала, Заполняется по результатм транзакции
        /// </summary>
        public string TerminalMessage;

        /// <summary>
        /// Сообщение об ошибке терминала, Заполняется по результатм транзакции
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// Заполняется по результатм транзакции.
        /// </summary>
        public int Status;

        public double Balance;

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.V("TRANS_ID", InternalId);

            switch (TransactionType)
            {
                case TranType.New:
                    sb.V("ACTION", "NEW_ORDER");
                    break;
                case TranType.Kill:
                    sb.V("ACTION", "KILL_ORDER");
                    break;
            }

            switch (Side)
            {
                case Side.Buy:
                    sb.V("OPERATION", "B");
                    break;
                case Side.Sell:
                    sb.V("OPERATION", "S");
                    break;
            }

            switch (ExecutionCondition)
            {
                case ExecCondiotion.FillOrKill:
                    sb.V("EXECUTION_CONDITION", "FILL_OR_KILL");
                    break;
                case ExecCondiotion.KillBalance:
                    sb.V("EXECUTION_CONDITION", "KILL_BALANCE");
                    break;
            }

            sb.V("CLASSCODE", ClassCode);
            sb.V("SECCODE", SecCode);
            sb.V("PRICE", Price);
            sb.V("ACCOUNT", Account);
            sb.V("QUANTITY", Qty);
            sb.V("CLIENT_CODE", ClientCode);
            sb.V("ORDER_KEY", OrderNum);
            sb.V("COMMENT", Comment);

            return sb.ToString();
        }
    }
}
