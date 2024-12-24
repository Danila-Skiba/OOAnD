using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class CommandInjectableCommandTests
    {
        [Fact]
        public void Positive_Execute_CommandInjectableCommand()
        {
            var commandMock = new Mock<ICommand>();
            var injectableCommand = new CommandInjectableCommand();
            injectableCommand.Inject(commandMock.Object);

            injectableCommand.Execute();

            commandMock.Verify(command => command.Execute(), Times.Once);
        }

        [Fact]
        public void Negative_Execute_CommandInjectableCommand()
        {
            var injectableCommand = new CommandInjectableCommand();

            Assert.Throws<InvalidOperationException>(() => injectableCommand.Execute());
        }
    }
}
