using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterAuthDependenciesTests : IDisposable
    {
        public RegisterAuthDependenciesTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }

        [Fact]
        public void Execute_RegistersAuthCommand()
        {
            var registerAuthDependencies = new RegisterAuthDependencies();

            registerAuthDependencies.Execute();

            var authCommand = Ioc.Resolve<ICommand>(
                "Commands.Auth",
                "player1",
                "ship1",
                "Fire"
            );

            Assert.IsType<AuthCommand>(authCommand);
        }
    }
}
