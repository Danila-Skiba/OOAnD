using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class IoCRegisterGameOperationTests : IDisposable
    {
        public IoCRegisterGameOperationTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void RegisterGameTest()
        {
            var reg = new IoCRegisterGameOperation();

            reg.Execute();

            var gameReceiver = Ioc.Resolve<object>("Game.Receiver");
            Assert.IsType<Game>(gameReceiver);
        }

        [Fact]
        public void ExecuteCommandsTest()
        {
            var reg = new IoCRegisterGameOperation();
            var cmd1 = new Mock<ICommand>();

            reg.Execute();
            var game = Ioc.Resolve<Game>("Game.Receiver");

            game.Receive(cmd1.Object);
            game.Execute();

            cmd1.Verify(cmd => cmd.Execute(), Times.Once);
        }
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
