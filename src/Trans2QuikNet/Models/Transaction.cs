using System.Text;
using Trans2QuikNet.Tools;

namespace Trans2QuikNet.Models
{
    public class Transaction
    {

        public Transaction() => InternalId = Utils.GetNextTrnId();

        public Transaction(int TrnId) => InternalId = TrnId;

        public ExecCondition ExecutionCondition;

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

            sb.AppendFieldAndValue("TRANS_ID", InternalId);

            switch (TransactionType)
            {
                case TranType.New:
                    sb.AppendFieldAndValue("ACTION", "NEW_ORDER");
                    break;
                case TranType.Kill:
                    sb.AppendFieldAndValue("ACTION", "KILL_ORDER");
                    break;
            }

            switch (Side)
            {
                case Side.Buy:
                    sb.AppendFieldAndValue("OPERATION", "B");
                    break;
                case Side.Sell:
                    sb.AppendFieldAndValue("OPERATION", "S");
                    break;
            }

            switch (ExecutionCondition)
            {
                case ExecCondition.FillOrKill:
                    sb.AppendFieldAndValue("EXECUTION_CONDITION", "FILL_OR_KILL");
                    break;
                case ExecCondition.KillBalance:
                    sb.AppendFieldAndValue("EXECUTION_CONDITION", "KILL_BALANCE");
                    break;
            }

            sb.AppendFieldAndValue("CLASSCODE", ClassCode);
            sb.AppendFieldAndValue("SECCODE", SecCode);
            sb.AppendFieldAndValue("PRICE", Price);
            sb.AppendFieldAndValue("ACCOUNT", Account);
            sb.AppendFieldAndValue("QUANTITY", Qty);
            sb.AppendFieldAndValue("CLIENT_CODE", ClientCode);
            sb.AppendFieldAndValue("ORDER_KEY", OrderNum);
            sb.AppendFieldAndValue("COMMENT", Comment);

            return sb.ToString();
        }
    }
}
