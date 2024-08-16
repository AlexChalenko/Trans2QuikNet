using Moq;
using System.Text;
using Trans2QuikNet.Delegates;
using Trans2QuikNet.Interfaces;
using Trans2QuikNet.Managers;
using Trans2QuikNet.Models;
using Xunit;


namespace Trans2QuikNet.Tests
{
    public class QuikConnectorTests
    {
        [Fact]
        public void Connector_ShouldCallNativeConnect_AndReturnSuccess_WhenDllFunctionsAreAvailable()
        {
            // Arrange
            var apiMock = new Mock<ITrans2QuikAPI>();
            apiMock.Setup(api => api.QuikPath).Returns("valid_path");

            var connectDelegateMock = new Mock<TRANS2QUIK_CONNECT>();
            connectDelegateMock.Setup(x => x(apiMock.Object.QuikPath, ref It.Ref<long>.IsAny, It.IsAny<StringBuilder>(), It.IsAny<uint>()))
                               .Returns(Result.SUCCESS);  // Предполагаем, что функция подключения возвращает успех

            apiMock.Setup(api => api.GetDelegate<TRANS2QUIK_CONNECT>("TRANS2QUIK_CONNECT"))
                   .Returns(connectDelegateMock.Object);  // Мокируем получение делегата

            var setConnectionStatusCallbackDelegateMock = new Mock<TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK>();
            var connectionStatusHandler = new Mock<TRANS2QUIK_CONNECTION_STATUS_CALLBACK>();


            setConnectionStatusCallbackDelegateMock.Setup(x => x(connectionStatusHandler.Object, ref It.Ref<long>.IsAny, It.IsAny<StringBuilder>(), It.IsAny<uint>()))
                                                   .Returns(Result.SUCCESS);  // Предполагаем, что функция установки обработчика статуса подключения возвращает успех

            apiMock.Setup(api => api.GetDelegate<TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK>("TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK"))
                .Returns(setConnectionStatusCallbackDelegateMock.Object);

            var connector = new QuikConnector(apiMock.Object);
            // Act
            var result = connector.Connect();

            // Assert
            Assert.True(result.Result == Result.SUCCESS);  // Утверждаем, что результат успешен
        }

        [Fact]
        public void Connector_ShouldCallNativeDisconnect_AndReturnSuccess_WhenDllFunctionsAreAvailable()
        {
            // Arrange
            var apiMock = new Mock<ITrans2QuikAPI>();
            apiMock.Setup(api => api.QuikPath).Returns("valid_path");

            //apiMock.Setup(api => api.GetDelegate<("TRANS2QUIK_CONNECT"))
            //       .Returns(new IntPtr(123456));  // Предполагаем, что адрес функции получен

            var connectDelegateMock = new Mock<TRANS2QUIK_CONNECT>();
            connectDelegateMock.Setup(x => x(apiMock.Object.QuikPath, ref It.Ref<long>.IsAny, It.IsAny<StringBuilder>(), It.IsAny<uint>()))
                               .Returns(Result.SUCCESS);  // Предполагаем, что функция подключения возвращает успех

            apiMock.Setup(api => api.GetDelegate<TRANS2QUIK_CONNECT>("TRANS2QUIK_CONNECT"))
                   .Returns(connectDelegateMock.Object);  // Мокируем получение делегата

            var disconnectDelegateMock = new Mock<TRANS2QUIK_DISCONNECT>();
            apiMock.Setup(api => api.GetDelegate<TRANS2QUIK_DISCONNECT>("TRANS2QUIK_DISCONNECT"))
                   .Returns(disconnectDelegateMock.Object);  // Мокируем получение делегата
            disconnectDelegateMock.Setup(x => x(ref It.Ref<long>.IsAny, It.IsAny<StringBuilder>(), It.IsAny<uint>()))
                               .Returns(Result.SUCCESS);  // Предполагаем, что функция отключения возвращает успех

            var setConnectionStatusCallbackDelegateMock = new Mock<TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK>();
            setConnectionStatusCallbackDelegateMock.Setup(x => x(It.IsAny<TRANS2QUIK_CONNECTION_STATUS_CALLBACK>(), ref It.Ref<long>.IsAny, It.IsAny<StringBuilder>(), It.IsAny<uint>()))
                                                   .Returns(Result.SUCCESS);  // Предполагаем, что функция установки обработчика статуса подключения возвращает успех

            apiMock.Setup(api => api.GetDelegate<TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK>("TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK"))
                .Returns(setConnectionStatusCallbackDelegateMock.Object);  // Мокируем получение делегата


            var connector = new QuikConnector(apiMock.Object);

            // Act
            var connectResult = connector.Connect();

            // Assert
            Assert.True(connectResult.Result == Result.SUCCESS);  // Утверждаем, что результат успешен

            var disconnectResult = connector.Disconnect();

            Assert.True(disconnectResult.Result == Result.SUCCESS);
        }
    }
}
