namespace Trans2QuikNet.Models
{
    public enum Side
    {
        Buy = 1,
        Sell = 2
    }

    public enum TranType
    {
        New = 1,
        Kill = 2
    }

    public enum ExecCondiotion
    {
        Queue = 1,        //Поставить в очередь
        FillOrKill = 1,   // Полностью или отклонить
        KillBalance = 3   //Снять остаток
    }

    public enum TimeConst
    {
        Date = 0,
        Time= 1,
        MicroSec = 2,
        WithdrawDate = 3,
        WithdrawTime = 4,
        WithdrawMicroSec = 5
    }
}
