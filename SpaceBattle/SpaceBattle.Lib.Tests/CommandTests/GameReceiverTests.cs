using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class GameReceiverTests : IDisposable
    {
        public GameReceiverTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Positive_Receive_AddsCommandToQueue()
        {
            var queue = new Queue<ICommand>();
            var receiver = new GameReceiver(queue);
            var commandMock = new Mock<ICommand>();

            receiver.Receive(commandMock.Object);

            Assert.Single(queue);
            Assert.Equal(commandMock.Object, queue.Dequeue());
        }

        [Fact]
        public void Positive_Receive_AcceptsNullCommand()
        {
            var queue = new Queue<ICommand>();
            var receiver = new GameReceiver(queue);

            receiver.Receive(null!);

            Assert.Single(queue);
            Assert.Null(queue.Dequeue());
        }

        [Fact]
        public void Negative_Constructor_ThrowsExceptionOnNullQueue()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new GameReceiver(null!));
            Assert.Equal("Value cannot be null. (Parameter 'queue')", exception.Message);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
