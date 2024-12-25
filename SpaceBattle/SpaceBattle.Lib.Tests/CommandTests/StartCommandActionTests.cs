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
            var injectableMock = new Mock<ICommand>().As<ICommandInjectable>();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.CommandInjectable", (object[] args) => injectableMock.Object).Execute();

            var injectableCmd = Ioc.Resolve<ICommand>("Commands.CommandInjectable");

            injectableMock.Object.Inject(command.Object);
            /* if (injectableCmd is ICommandInjectable injectable)
             {
                 injectable.Inject(command.Object);
             }*/

            var receiver = new Mock<ICommandReceiver>();
            var gameObject = new Mock<IDictionary<string, object>>();
            var cmdName = "TestCommand";

            var startCmd = new StartCommand(injectableCmd, receiver.Object, gameObject.Object, cmdName);
            startCmd.Execute();

            gameObject.VerifySet(obj => obj[cmdName] = injectableCmd, Times.Once);
            receiver.Verify(r => r.Receive(injectableCmd), Times.Once);
        }

        [Fact]
        public void Execute_IfCommandException()
        {

            var command = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var gameObject = new Mock<IDictionary<string, object>>();
            var cmdName = "TestCommand";

            gameObject.SetupSet(obj => obj[cmdName] = It.IsAny<ICommand>()).Throws<Exception>();

            var startCommand = new StartCommand(command.Object, receiver.Object, gameObject.Object, cmdName);

            Assert.Throws<Exception>(() => startCommand.Execute());
            gameObject.VerifySet(obj => obj[cmdName] = command.Object, Times.Once);
        }

        [Fact]
        public void Execute_ShouldNotAddСommandToGameObjectIfReceiveThrowsException()
        {

            var Command = new Mock<ICommand>();
            var Receiver = new Mock<ICommandReceiver>();
            var GameObject = new Mock<IDictionary<string, object>>();
            var commandName = "TestCommand";

            Receiver.Setup(r => r.Receive(It.IsAny<ICommand>())).Throws<Exception>();

            var startCommand = new StartCommand(Command.Object, Receiver.Object, GameObject.Object, commandName);

            Assert.Throws<Exception>(() => startCommand.Execute());
            GameObject.Verify(g => g.Add(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
