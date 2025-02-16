using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencySendCommandTest: IDisposable
    {
        public RegisterIoCDependencySendCommandTest()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }
        [Fact]
        public void Execute_ShouldRegisterIoCDependencySendCommand()
        {
            // Arrange

            var command = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();

            // Act
            new RegisterIoCDependencySendCommand().Execute();
            var res = Ioc.Resolve<ICommand>("Commands.Send", receiver.Object, command.Object);

            // Assert
            Assert.IsType<SendCommand>(res);
        }
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
