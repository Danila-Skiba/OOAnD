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
        public void Receive_AddsCommandToQueue()
        {
            var queue = new Queue<ICommand>();
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.CommandsQueue",
                (object[] args) => queue
            ).Execute();

            var receiver = new GameReceiver();
            var commandMock = new Mock<ICommand>();

            receiver.Receive(commandMock.Object);

            Assert.Single(queue);
            Assert.Equal(commandMock.Object, queue.Dequeue());
        }

        [Fact]
        public void Receive_AcceptsNullCommand()
        {
            var queue = new Queue<ICommand>();
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.CommandsQueue",
                (object[] args) => queue
            ).Execute();

            var receiver = new GameReceiver();

            receiver.Receive(null);

            Assert.Single(queue);
            Assert.Null(queue.Dequeue());
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
