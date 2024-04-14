using Trans2QuikNet.Models;
using Xunit;

namespace Trans2QuikNet.Tests
{
    public class RealQuik_Transaction_Tests
    {
        private string realPathToQuik = "C:\\QUIK_SBER\\";

        [Fact]
        public void SendTranasction_Successful()
        {
            using var api = new Trans2QuikAPI(realPathToQuik);
            var connector = new QuikConnector(api);
            connector.Connect();

            var transactionManager = new QuikTransactionManager(api);

            var transaction = new Transaction(101)
            {
                Account = "L01-00000F00",
                ClassCode = "TQBR",
                ExecutionCondition = ExecCondiotion.Queue,
                Price = 300,
                Qty = 1,
                SecCode = "SBER",
                Side = Side.Buy,
                ClientCode = "420GPTR"
            };

            transactionManager.OnTransactionReplyReceived += (sender, args) =>
            {
                Assert.Equal(Result.SUCCESS, args.TransactionResult);
                Assert.Equal(0, args.TransactionReplyCode);
                Assert.Equal("Успешно", args.TransactionReplyMessage);
            };

            var exception = Record.Exception(() => transactionManager.SendTransaction(transaction));
            Assert.Null(exception);

            connector.Disconnect();
        }

        [Fact]
        public void SendAsyncTransaction_Successful()
        {
            bool eventFired = false;

            using var api = new Trans2QuikAPI(realPathToQuik);
            var connector = new QuikConnector(api);
            connector.Connect();

            var transactionManager = new QuikTransactionManager(api);
            var transaction = new Transaction(1235)
            {
                Account = "L01-00000F00",
                ClassCode = "TQBR",
                ExecutionCondition = ExecCondiotion.Queue,
                Price = 123,
                Qty = 1,
                SecCode = "SBER",
                Side = Side.Buy,
                TransactionType = TranType.New
            };

            transactionManager.OnTransactionReplyReceived += (sender, args) =>
            {
                eventFired = true;
                //Assert.Equal(Result.SUCCESS, args.TransactionResult);
                //Assert.Equal(0, args.TransactionReplyCode);
                //Assert.Equal("Успешно", args.TransactionReplyMessage);
            };

            var exception = Record.Exception(() => transactionManager.SendTranactionAsync(transaction));
            Assert.Null(exception);

            Assert.True(eventFired, "TransactionReply event was not fired");
            connector.Disconnect();
        }

        //[Fact]
        //public void RegisterTransactionReplyCallback_Successful()
        //{
        //    using var api = new Trans2QuikAPI(realPathToQuik);
        //    var transactionManager = new QuikTransactionManager(api);

        //    var exception = Record.Exception(() => transactionManager.RegisterTransactionReplyCallback());
        //    Assert.Null(exception);
        //}
    }
}
