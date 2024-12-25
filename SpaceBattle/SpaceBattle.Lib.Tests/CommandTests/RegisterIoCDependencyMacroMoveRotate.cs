using App;
using App.Scopes;
using Moq;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyMacroMoveRotateTests : IDisposable
    {
        public RegisterIoCDependencyMacroMoveRotateTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }
        [Fact]
        public void Execute_RegistersMacroMoveAndMacroRotateCommands()
        {

            var registerMc = new RegisterIoCDependencyMacroCommand();
            registerMc.Execute();

            var registerCommand = new RegisterIoCDependencyMacroMoveRotate();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Move", (object[] args) => new[] { "Commands.Move" }).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Rotate", (object[] args) => new[] { "Commands.Rotate" }).Execute();

            var moveMock = new Mock<ICommand>();
            var rotateMock = new Mock<ICommand>();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Move", (object[] args) => moveMock.Object).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Rotate", (object[] args) => rotateMock.Object).Execute();
            registerCommand.Execute();

            var macroMoveCommand = Ioc.Resolve<ICommand>("Macro.Move", new object[] { new object[] { "arg1" } });
            var macroRotateCommand = Ioc.Resolve<ICommand>("Macro.Rotate", new object[] { new object[] { "arg2" } });

            Assert.NotNull(macroMoveCommand);
            Assert.NotNull(macroRotateCommand);

            Assert.IsType<MacroCommand>(macroMoveCommand);
            Assert.IsType<MacroCommand>(macroRotateCommand);
        }
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
