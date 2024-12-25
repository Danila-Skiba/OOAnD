using App;
using App.Scopes;
namespace SpaceBattle.Lib.Tests
{
    public class RegisterDependencyCommandInjectableCommandTest : IDisposable
    {
        public RegisterDependencyCommandInjectableCommandTest()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void PositiveRegisterDependencyCommandInjectableCommandTest()
        {
            var reg_command = new RegisterDependencyCommandInjectableCommand();
            reg_command.Execute();

            var command = Ioc.Resolve<ICommand>("Commands.CommandInjectable");
            var commandInjectable = Ioc.Resolve<ICommandInjectable>("Commands.CommandInjectable");
            var commandInjectableCommand = Ioc.Resolve<CommandInjectableCommand>("Commands.CommandInjectable");

            Assert.IsType<CommandInjectableCommand>(command);
            Assert.IsType<CommandInjectableCommand>(commandInjectable);
            Assert.IsType<CommandInjectableCommand>(commandInjectableCommand);
        }

        [Fact]
        public void NegativeRegisterDependencyCommandInjectableCommandTest()
        {
            Assert.Throws<Exception>(() => Ioc.Resolve<ICommand>("Commands.CommadInjectable"));
            Assert.Throws<Exception>(() => Ioc.Resolve<ICommandInjectable>("Commands.CommadInjectable"));
            Assert.Throws<Exception>(() => Ioc.Resolve<CommandInjectableCommand>("Commands.CommadInjectable"));
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
