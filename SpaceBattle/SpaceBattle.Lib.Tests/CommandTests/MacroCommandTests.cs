using Moq;
using SpaceBattle.Lib.Commands;

namespace SpaceBattle.Lib.Tests.CommandTests
{
    public class MacroCommandTests
    {
        [Fact]
        public void Execute_AllCommandsExecuted()
        {
            var cmd1 = new Mock<ICommand>();
            var cmd2 = new Mock<ICommand>();
            var macro = new MacroCommand(new ICommand[] { cmd1.Object, cmd2.Object });

            macro.Execute();

            cmd1.Verify(c => c.Execute(), Times.Once());
            cmd2.Verify(c => c.Execute(), Times.Once());
        }

        [Fact]
        public void Execute_ExceptionInOneCommand_StopsExecution()
        {
            var cmd1 = new Mock<ICommand>();
            var cmd2 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Throws(new Exception("Test"));

            var macro = new MacroCommand(new ICommand[] { cmd1.Object, cmd2.Object });
            Assert.Throws<Exception>(() => macro.Execute());
            cmd2.Verify(c => c.Execute(), Times.Never());
        }

        [Fact]
        public void Constructor_NullCommands_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MacroCommand(null));
        }

        [Fact]
        public void Execute_EmptyCommandsArray_NoExceptions()
        {
            var macro = new MacroCommand(Array.Empty<ICommand>());
            var exception = Record.Exception(() => macro.Execute());
            Assert.Null(exception);
        }
    }
}
