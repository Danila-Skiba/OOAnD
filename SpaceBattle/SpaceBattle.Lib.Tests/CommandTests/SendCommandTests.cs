using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class SendCommandTest
    {
        [Fact]
        public void Execute_ShouldPutCommandIntoReceiver()
        {
            // Arrange
            var cmd = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var sendCommand = new SendCommand(cmd.Object, receiver.Object);

            // Act
            sendCommand.Execute();

            // Assert
            receiver.Verify(r => r.Receive(cmd.Object), Times.Once);
        }

        [Fact]
        public void Execute_ShouldThrowException_WhenReceiverCannotReceiveCommand()
        {
            // Arrange
            var cmd = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var sendCommand = new SendCommand(cmd.Object, receiver.Object);

            // Настройка мока для выброса исключения на конкретном аргументе
            receiver.Setup(r => r.Receive(cmd.Object)).Throws(new Exception());

            // Act & Assert
            Assert.Throws<Exception>(() => sendCommand.Execute());
        }
    }
}

