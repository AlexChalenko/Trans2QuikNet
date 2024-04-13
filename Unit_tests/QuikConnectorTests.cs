using Trans2QuikNet.Models;
using Xunit;


namespace Trans2QuikNet.Tests
{

    public class QuikConnectorTests /*: IClassFixture<QuikConnectorFixture>*/
    {

        //private readonly QuikConnector _connector;
        //private readonly Trans2QuikAPI _api;
        private readonly string realPathToQuik = "C:\\QUIK_SBER\\";
        private readonly string fakePathToQuik = "C:\\QUIK_SBERq\\";

        //public QuikConnectorTests()
        //{
        //    _api = new Trans2QuikAPI(realPathToQuik);
        //    _connector = new QuikConnector(_api);
        //}

        //REAL CONNECTION
        //private string _pathToQuik = "C:\\QUIK_SBER\\";

        [Fact]
        public void Connect_Successful()
        {
            using var api = new Trans2QuikAPI(realPathToQuik);
            var connector = new QuikConnector(api);

            var connectException = Record.Exception(connector.Connect);

            Assert.Null(connectException);
            //Assert.True(connector.IsDllConnected());
            connector.Disconnect();
        }

        [Fact]
        public void Connect_Disconnect_Successful()
        {
            using var api = new Trans2QuikAPI(realPathToQuik);
            var connector = new QuikConnector(api);

            var exception = Record.Exception(connector.Connect);

            Assert.Null(exception);

            var disException = Record.Exception(connector.Disconnect);
            Assert.Null(disException);
        }

        [Fact]
        public void Connect_Failed()
        {
            var exception = Record.Exception(() =>
            {
                using var api = new Trans2QuikAPI(fakePathToQuik);
            });

            Assert.NotNull(exception);
        }

        [Fact]
        public void OnConnectionStatus_Fires_Correctly()
        {
            // Arrange
            bool eventFired = false;
            Result expectedEvent = Result.DLL_CONNECTED;
            int expectedErrorCode = 0;
            string expectedMessage = "";

            using var api = new Trans2QuikAPI(realPathToQuik);
            var connector = new QuikConnector(api);

            connector.OnConnectionStatusChanged += (sender, e) =>
            {
                var api = new Trans2QuikAPI(realPathToQuik);
                var connector = new QuikConnector(api);
                eventFired = true;
                Assert.Equal(expectedEvent, e.ConnectionEvent);
                Assert.Equal(expectedErrorCode, e.ExtendedErrorCode);
                Assert.Equal(expectedMessage, e.ErrorMessage);
            };

            // Simulate the event trigger from the native API
            connector.Connect();

            // Assert
            Assert.True(eventFired, "The connection status event should have been fired.");

            connector.Disconnect();
        }

        //TODO : MemoryLeakTest
        //[Fact]
        //public void MemoryLeakTest()
        //{
        //    // Arrange
        //    using var api = new Trans2QuikAPI(realPathToQuik);
        //    var connector = new QuikConnector(api);
        //    connector.Connect();

        //    // Act
        //    connector.Disconnect();
        //    api.Dispose();

        //    // Assert
        //    //Assert.True(api.Dispose);
        //}


        //public void Dispose()
        //{
        //    // Очистка после каждого теста
        //    //_api.Dispose();

        //    // Здесь можно добавить другие действия по очистке
        //    Console.WriteLine("Cleanup after each test");
        //}
    }
}
