using System.Reflection;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class GameTests
    {
        [Fact]
        public void ExecuteAllCommands()
        {
            var game = new Game();
            var cmd1 = new Mock<ICommand>();
            var cmd2 = new Mock<ICommand>();
            game.Receive(cmd1.Object);
            game.Receive(cmd2.Object);
            game.Execute();
            cmd1.Verify(c => c.Execute(), Times.Once);
            cmd2.Verify(c => c.Execute(), Times.Once);
        }

        [Fact]
        public void ExecuteQueueIsEmpty()
        {
            var game = new Game();
            game.Execute();
            Assert.True(true);
        }

        [Fact]
        public void ExecuteCommandsException()
        {
            var game = new Game();
            var cmd1 = new Mock<ICommand>();
            var cmd2 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Throws(new Exception("Test exception"));
            game.Receive(cmd1.Object);
            game.Receive(cmd2.Object);
            game.Execute();
            cmd1.Verify(c => c.Execute(), Times.Once);
            cmd2.Verify(c => c.Execute(), Times.Once);
        }

        [Fact]
        public void ExceptionCommandNull()
        {
            var game = new Game();
            var exception = Assert.Throws<ArgumentNullException>(() => game.Receive(null!));
            Assert.Equal("cmd", exception.ParamName);
        }

        [Fact]
        public void Execute_NullCommandInQueue_ThrowsInvalidOperationException()
        {
            var game = new Game();
            var commandsField = typeof(Game).GetField("_commands", BindingFlags.NonPublic | BindingFlags.Instance);
            if (commandsField is null)
            {
                throw new InvalidOperationException("Поле _commands не найдено");
            }

            var value = commandsField.GetValue(game);
            if (value is not Queue<ICommand> commandsQueue)
            {
                throw new InvalidOperationException("Поле _commands не имеет ожидаемого типа");
            }

            ICommand? nullCommand = null;
            commandsQueue.Enqueue(nullCommand!);
            var exception = Assert.Throws<InvalidOperationException>(() => game.Execute());
            Assert.Equal("Обнаружена null-команда в очереди.", exception.Message);
        }

        [Fact]
        public void Execute_CommandsAreExecutedInFIFOOrder()
        {
            var game = new Game();
            var executionOrder = new List<string>();
            var cmd1 = new Mock<ICommand>();
            cmd1.Setup(c => c.Execute()).Callback(() => executionOrder.Add("первый"));
            var cmd2 = new Mock<ICommand>();
            cmd2.Setup(c => c.Execute()).Callback(() => executionOrder.Add("второй"));
            game.Receive(cmd1.Object);
            game.Receive(cmd2.Object);
            game.Execute();
            Assert.Equal(new List<string> { "первый", "второй" }, executionOrder);
        }
    }
}
