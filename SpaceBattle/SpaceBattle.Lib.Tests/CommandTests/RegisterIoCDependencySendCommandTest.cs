using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencySendCommandTest
    {
        [Fact]
        public void Execute_ShouldRegisterIoCDependencySendCommand()
        {
            // Arrange
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

            var command = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();

            // Act
            new RegisterIoCDependencySendCommand().Execute();
            var res = Ioc.Resolve<ICommand>("Commands.Send", receiver.Object, command.Object);

            // Assert
            Assert.IsType<SendCommand>(res);
        }
    }
}
