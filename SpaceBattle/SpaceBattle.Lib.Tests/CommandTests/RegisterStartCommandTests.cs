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
            var gameObject = new Dictionary<string, object>();
            var order = new Dictionary<string, object>
            {
                {"Cmd", injectableCommandMock.Object},
                {"Receiver", receiverMock.Object},
                {"Name", "TestCommand"},
                {"Object", gameObject}
            };

            var startCommand = Ioc.Resolve<ICommand>("Actions.Start", order);
            startCommand.Execute();

            Assert.IsType<StartCommand>(startCommand);
            Assert.True(gameObject.ContainsKey("TestCommand"));
            receiverMock.Verify(r => r.Receive(It.IsAny<ICommand>()), Times.Once);
        }

        [Fact]
        public void Test_RegisterIoCDependencyActionsStart_DependencyNotResolved()
        {
            var commandMock = new Mock<ICommand>();
            var receiverMock = new Mock<ICommandReceiver>();
            var gameObject = new Dictionary<string, object>();
            var order = new Dictionary<string, object>
            {
                { "Cmd", commandMock.Object },
                { "Receiver", receiverMock.Object },
                { "Name", "TestCommand" },
                { "Object", gameObject }
            };

            var registerCommand = new RegisterIocDependencyActionsStart();

            Assert.Throws<Exception>(() => Ioc.Resolve<ICommand>("Actions.Start", order));
            Assert.False(gameObject.ContainsKey("TestCommand"));
            receiverMock.Verify(r => r.Receive(It.IsAny<ICommand>()), Times.Never);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
