using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyActionsStartTests : IDisposable
    {
        public RegisterIoCDependencyActionsStartTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void RegisterIoCDependencyActionsStart_ResolvesDependency()
        {

            var registerCommand = new RegisterIocDependencyActionsStart();
            registerCommand.Execute();

            var injectableCommandMock = new Mock<ICommand>();
            var receiverMock = new Mock<ICommandReceiver>();
            var gameObject = new Mock<IDictionary<string, object>>();
            var cmdName = "TestCommand";
            var order = new Dictionary<string, object>
            {
                {"Cmd", injectableCommandMock.Object},
                {"Receiver", receiverMock.Object},
                {"Name", cmdName},
                {"Object", gameObject.Object}
            };

            var startCommand = Ioc.Resolve<ICommand>("Actions.Start", order);
            startCommand.Execute();

            Assert.IsType<StartCommand>(startCommand);
            gameObject.VerifySet(obj => obj[cmdName] = injectableCommandMock.Object, Times.Once);
            receiverMock.Verify(r => r.Receive(injectableCommandMock.Object), Times.Once);
        }

        [Fact]
        public void Test_RegisterIoCDependencyActionsStart_DependencyNotResolved()
        {
            var command = new Mock<ICommand>();
            var receiver = new Mock<ICommandReceiver>();
            var gameObject = new Mock<IDictionary<string, object>>();
            var cmdName = "TestCommand";

            var order = new Dictionary<string, object>
            {
                { "Cmd", command.Object },
                { "Receiver", receiver.Object },
                { "Name", cmdName },
                { "Object", gameObject.Object }
            };

            var registerCommand = new RegisterIocDependencyActionsStart();

            Assert.Throws<Exception>(() => Ioc.Resolve<ICommand>("Actions.Start", order));
            gameObject.VerifySet(obj => obj[cmdName] = command.Object, Times.Never);
            receiver.Verify(r => r.Receive(command.Object), Times.Never);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
