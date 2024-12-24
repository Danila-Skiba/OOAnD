using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class StartCommandTests : IDisposable
    {
        public StartCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }
        [Fact]
        public void Execute_addCommandtoGameObjandSendtoReceiver()
        {
            var command = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var gameObject = new Mock<IDictionary<string, object>>();
            var cmdName = "TestCommand";

            var startCommand = new StartCommand(command.Object, receiver.Object, gameObject.Object, cmdName);
            startCommand.Execute();

            gameObject.VerifySet(obj => obj[cmdName] = command.Object);
            receiver.Verify(r => r.Receive(command.Object), Times.Once);
        }

        [Fact]
        public void Execute_AddInjectableCommandToGameObjAndSendToReceiver()
        {
            var command = new Mock<ICommand>();
            var registerCmd = new RegisterDependencyCommandInjectableCommand();
            registerCmd.Execute();

            var injectableCmd = Ioc.Resolve<ICommand>("Commands.CommandInjectable");
            if (injectableCmd is ICommandInjectable injectable)
            {
                injectable.Inject(command.Object);
            }

            var receiver = new Mock<ICommandReceiver>();
            var gameObject = new Mock<IDictionary<string, object>>();
            var cmdName = "TestCommand";

            var startCmd = new StartCommand(injectableCmd, receiver.Object, gameObject.Object, cmdName);
            startCmd.Execute();

            gameObject.VerifySet(obj => obj[cmdName] = injectableCmd, Times.Once);
            receiver.Verify(r => r.Receive(injectableCmd), Times.Once);
        }

        [Fact]
        public void Execute_NotReceiveCommandIfException()
        {

            var command = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var gameObject = new Mock<IDictionary<string, object>>();
            var cmdName = "TestCommand";

            gameObject.SetupSet(obj => obj[cmdName] = It.IsAny<ICommand>()).Throws<Exception>();

            var startCommand = new StartCommand(command.Object, receiver.Object, gameObject.Object, cmdName);

            Assert.Throws<Exception>(() => startCommand.Execute());
            gameObject.VerifySet(obj => obj[cmdName] = command.Object, Times.Once);
            receiver.Verify(r => r.Receive(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public void Execute_ShouldCallRemoveOnGameObjectWhenExceptionIsThrown()
        {
            var command = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var gameObject = new Mock<IDictionary<string, object>>();
            var cmdName = "TestCommand";

            receiver.Setup(r => r.Receive(It.IsAny<ICommand>())).Throws<Exception>();

            var startCommand = new StartCommand(command.Object, receiver.Object, gameObject.Object, cmdName);
            startCommand.Execute();

            gameObject.Verify(g => g.Remove(cmdName), Times.Once);
        }
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
