using Trans2QuikNet;
using Trans2QuikNet.Models;
using Trans2QuikNet.OrderManager;
using Trans2QuikNet.TradeManager;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var quikPath = @"C:\QUIK-Junior\"; // Укажите реальный путь к установленному терминалу QUIK
            var quikPath = @"C:\QUIK-Junior-Latest\"; // Укажите реальный путь к установленному терминалу QUIK

            //var quikService = new QuikService(quikPath);


            try
            {
                using var api = new Trans2QuikAPI(quikPath);

                using var connector = new QuikConnector(api);
                using var transactionManager = new QuikTransactionManager(api);
                using var orderManager = new QuikOrderManager(api);
                using var tradeManager = new QuikTradeManager(api);


                transactionManager.OnTransactionReplyReceived += (sender, args) =>
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("OnTransactionReply\n");
                    Console.WriteLine(args);
                };

                connector.Connect();

                Console.WriteLine($"IsDllConnected: {connector.IsDllConnected}");
                Console.WriteLine($"IsQuikConnected: {connector.IsQuikConnected}");

                var r = orderManager.SubscribeOrders("QJSIM", "");
                var q = orderManager.StartOrders();

                orderManager.OnOrderStatusReceived += (sender, args) =>
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("OnOrderStatusReceived\n");
                    Console.WriteLine(args);
                };

                var t1 = tradeManager.SubscribeTrades("QJSIM", "");
                var t2 = tradeManager.StartTrades();

                tradeManager.OnTradeStatusReceived += (sender, args) =>
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("OnTradeStatusReceived\n");
                    Console.WriteLine(args);
                };

                var transaction = new Transaction(101)
                {
                    Account = "NL0011100043",
                    ClassCode = "QJSIM",
                    Price = 300,
                    Qty = 1,
                    SecCode = "SBER",
                    Side = Side.Buy,
                    ClientCode = "10952"
                };

                transactionManager.SendTranactionAsync(transaction);

                Console.WriteLine($"IsDllConnected: {connector.IsDllConnected}");
                Console.WriteLine($"IsQuikConnected: {connector.IsQuikConnected}");

                Console.WriteLine("ok");
                Console.Read();
                connector.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
