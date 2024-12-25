using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyRotateCommandTests : IDisposable
    {

        public RegisterIoCDependencyRotateCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Execute_ShouldRegisterRotateCommandDependencyTrue()
        {

            var rotatingMock = new Mock<IRotating>();
            var mockGameObject = new Mock<object>();
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Adapters.IRotatingObject",
                (object[] args) => rotatingMock.Object
            ).Execute();

            var register = new RegisterIoCDependencyRotateCommand();
            register.Execute();

            var resolvedCommand = Ioc.Resolve<ICommand>("Commands.Rotate", mockGameObject.Object);
            Assert.NotNull(resolvedCommand);
            Assert.IsType<Rotate>(resolvedCommand);
        }

        [Fact]
        public void Execute_ShouldNotFindRotateCommandDependencyInNewScope()
        {
            var mockGameObject = new Mock<object>();

            Assert.Throws<Exception>(() => Ioc.Resolve<ICommand>("Commands.Rotate", mockGameObject.Object));
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
