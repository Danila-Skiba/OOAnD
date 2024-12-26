using App;
using Moq;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests
{
    public class RegisterIoCDependencyMacroCommandTests : IDisposable
    {
        public RegisterIoCDependencyMacroCommandTests()
        {
            //инициализируем IoC
            new App.Scopes.InitCommand().Execute();
            var newScope = Ioc.Resolve<System.Collections.Generic.IDictionary<string, Func<object[], object>>>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", newScope).Execute();
        }

        [Fact]
        public void Execute_Should_Register_CommandsMacro_Dependency_WithNoArgs()
        {
            //Arrange
            var registerCommand = new RegisterIoCDependencyMacroCommand();

            //Act
            registerCommand.Execute();

            //Assert
            var resolvedCommand = Ioc.Resolve<ICommand>("Commands.Macro");
            Assert.NotNull(resolvedCommand);
            Assert.IsType<MacroCommand>(resolvedCommand);

            var macroCommand = resolvedCommand as MacroCommand;
            Assert.NotNull(macroCommand);
        }

        [Fact]
        public void Execute_Should_Register_CommandsMacro_Dependency_WithCommands()
        {
            //Arrange
            var registerCommand = new RegisterIoCDependencyMacroCommand();
            var cmd1 = new Mock<ICommand>();
            var cmd2 = new Mock<ICommand>();
            var commands = new ICommand[] { cmd1.Object, cmd2.Object };

            //Act
            registerCommand.Execute();
            var resolvedCommand = Ioc.Resolve<ICommand>("Commands.Macro", (object)commands);

            //Assert
            Assert.NotNull(resolvedCommand);
            Assert.IsType<MacroCommand>(resolvedCommand);

            var macroCommand = resolvedCommand as MacroCommand;
            Assert.NotNull(macroCommand);

            //провка макрокоманда выполняются
            macroCommand.Execute();
            cmd1.Verify(c => c.Execute(), Times.Once());
            cmd2.Verify(c => c.Execute(), Times.Once());
        }

        [Fact]
        public void Execute_Should_ThrowException_When_Args0_IsNull()
        {
            // Arrange
            var registerCommand = new RegisterIoCDependencyMacroCommand();
            var args = new object?[] { null }; // Изменено на object?[]

            // Act
            registerCommand.Execute();

            // Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                Ioc.Resolve<ICommand>("Commands.Macro", args!)
            );
            Assert.Equal("Invalid arguments for Commands.Macro", exception.Message);
        }

        [Fact]
        public void Execute_Should_ThrowException_When_Args0_IsInvalidType()
        {
            //Arrange
            var registerCommand = new RegisterIoCDependencyMacroCommand();
            var args = new object[] { "InvalidArgument" };

            //Act
            registerCommand.Execute();

            //Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                Ioc.Resolve<ICommand>("Commands.Macro", args)
            );
            Assert.Equal("Invalid arguments for Commands.Macro", exception.Message);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
