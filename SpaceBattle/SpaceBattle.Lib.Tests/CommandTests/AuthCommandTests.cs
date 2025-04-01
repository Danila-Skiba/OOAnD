using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class AuthCommandTests : IDisposable
    {
        public AuthCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

            var cmd = new Mock<ICommand>();
            var gameItems = new Dictionary<string, object>();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Item.Add", (object[] args) =>
            {
                var id = (string)args[0];
                var item = args[1];
                gameItems[id] = item;
                return cmd.Object;
            }).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Item.Get", (object[] args) => gameItems[(string)args[0]]).Execute();
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }

        [Fact]
        public void Execute_Valid_Success()
        {
            var playerId = "player1";
            var objectId = "ship1";
            var operation = "Fire";

            Ioc.Resolve<ICommand>("Game.Item.Add", $"{objectId}_permissions", new Dictionary<string, string>() { { playerId, operation } }).Execute();

            var authCommand = new AuthCommand(playerId, objectId, operation);

            var exception = Record.Exception(authCommand.Execute);

            Assert.Null(exception);
        }

        [Fact]
        public void Execute_MissingPermission_ThrowsException()
        {
            var playerId = "player1";
            var objectId = "ship1";
            var operation = "Fire";

            Ioc.Resolve<ICommand>("Game.Item.Add", $"{objectId}_permissions", new Dictionary<string, string> { { playerId, "Move" } }).Execute();

            var authCommand = new AuthCommand(playerId, objectId, operation);

            var exception = Assert.Throws<UnauthorizedAccessException>(authCommand.Execute);
            Assert.Equal($"Player {playerId} is not authorized to perform operation '{operation}' on object {objectId}.", exception.Message);
        }

        [Fact]
        public void Execute_EmptyPermissions_ThrowsException()
        {
            var playerId = "player1";
            var objectId = "ship1";
            var operation = "Fire";

            Ioc.Resolve<ICommand>("Game.Item.Add", $"{objectId}_permissions", new Dictionary<string, string>()).Execute();

            var authCommand = new AuthCommand(playerId, objectId, operation);

            var exception = Assert.Throws<UnauthorizedAccessException>(authCommand.Execute);
            Assert.Equal($"Player {playerId} is not authorized to perform operation '{operation}' on object {objectId}.", exception.Message);
        }
    }
}
