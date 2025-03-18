using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class GameTests : IDisposable
{
    public GameTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void ExecuteCommandsTest()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.TimeQuant",
            (object[] args) => (object)TimeSpan.FromMilliseconds(50)
        ).Execute();

        var game_queue = new Queue<SpaceBattle.Lib.ICommand>();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.CommandsQueue",
            (object[] args) => game_queue
        ).Execute();

        var game_scope = Ioc.Resolve<object>("IoC.Scope.Current");

        var cmd1 = new Mock<SpaceBattle.Lib.ICommand>();
        cmd1.Setup(c => c.Execute());
        var cmd2 = new Mock<SpaceBattle.Lib.ICommand>();
        cmd2.Setup(c => c.Execute());

        game_queue.Enqueue(cmd1.Object);
        game_queue.Enqueue(cmd2.Object);

        var game = new Game(game_scope);

        game.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
    }

    [Fact]
    public void ExecuteEmptyQueueTest()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.TimeQuant",
            (object[] args) => (object)TimeSpan.FromMilliseconds(50)
        ).Execute();

        var game_queue = new Queue<SpaceBattle.Lib.ICommand>();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.CommandsQueue",
            (object[] args) => game_queue
        ).Execute();

        var game_scope = Ioc.Resolve<object>("IoC.Scope.Current");
        var game = new Game(game_scope);

        var exception = Record.Exception(() => game.Execute());
        Assert.Null(exception);
    }

    [Fact]
    public void ExecuteCommandsTimeExceededTest()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.TimeQuant",
            (object[] args) => (object)TimeSpan.FromMilliseconds(10)
        ).Execute();

        var game_queue = new Queue<SpaceBattle.Lib.ICommand>();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.CommandsQueue",
            (object[] args) => game_queue
        ).Execute();

        var game_scope = Ioc.Resolve<object>("IoC.Scope.Current");

        var cmd1 = new Mock<SpaceBattle.Lib.ICommand>();
        cmd1.Setup(c => c.Execute()).Callback(() => Thread.Sleep(20));
        var cmd2 = new Mock<SpaceBattle.Lib.ICommand>();
        cmd2.Setup(c => c.Execute());

        game_queue.Enqueue(cmd1.Object);
        game_queue.Enqueue(cmd2.Object);

        var game = new Game(game_scope);

        game.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Never());
    }

    [Fact]
    public void ExecuteCommandExceptionTest()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.TimeQuant",
            (object[] args) => (object)TimeSpan.FromMilliseconds(50)
        ).Execute();

        var game_queue = new Queue<SpaceBattle.Lib.ICommand>();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.CommandsQueue",
            (object[] args) => game_queue
        ).Execute();

        var game_scope = Ioc.Resolve<object>("IoC.Scope.Current");

        var cmd1 = new Mock<SpaceBattle.Lib.ICommand>();
        cmd1.Setup(c => c.Execute()).Throws(new Exception());
        var cmd2 = new Mock<SpaceBattle.Lib.ICommand>();
        cmd2.Setup(c => c.Execute());

        game_queue.Enqueue(cmd1.Object);
        game_queue.Enqueue(cmd2.Object);

        var game = new Game(game_scope);

        game.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
    }

    public void Dispose()
    {
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
    }
}
