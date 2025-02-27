using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyEmptyCommandTests : IDisposable
    {
        public RegisterIoCDependencyEmptyCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void ShouldRegisterEmptyCommand()
        {
            new RegisterIoCDependencyEmptyCommand().Execute();
            Assert.IsType<EmptyCommand>(Ioc.Resolve<ICommand>("Commands.Empty"));
        }
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
