using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class SendCommandTest
    {
        [Fact]
        public void Execute_ShouldPutCommandIntoReceiver()
        {
            var cmd = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var sendCommand = new SendCommand(cmd.Object, receiver.Object);

            sendCommand.Execute();

            receiver.Verify(r => r.Receive(cmd.Object), Times.Once);
        }

        [Fact]
        public void Execute_ShouldThrowException_WhenReceiverCannotReceiveCommand()
        {
            var cmd = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var sendCommand = new SendCommand(cmd.Object, receiver.Object);

            receiver.Setup(r => r.Receive(cmd.Object)).Throws(new Exception());

            Assert.Throws<Exception>(() => sendCommand.Execute());
        }
    }
}

