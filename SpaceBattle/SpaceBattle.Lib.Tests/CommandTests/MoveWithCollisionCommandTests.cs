using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class MoveWithCollisionCommandTests : IDisposable
    {
        public MoveWithCollisionCommandTests()
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
        public void Execute_NoNearbyObjects_NoCollisionChecks()
        {
            var movingMock = new Mock<IMoving>();
            var moveCommandMock = new Mock<ICommand>();
            var emptyObjects = Enumerable.Empty<object>();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Move", (object[] args) => moveCommandMock.Object).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.GetNearbyObjects", (object[] args) => emptyObjects).Execute();

            var command = new MoveWithCollisionCommand(movingMock.Object);

            command.Execute();

            moveCommandMock.Verify(cmd => cmd.Execute());
        }

        [Fact]
        public void Execute_WithNearbyObjects_CollisionCheckForEach()
        {
            var movingMock = new Mock<IMoving>();
            var moveCommandMock = new Mock<ICommand>();
            var collisionCheckMock = new Mock<ICommand>();
            var nearbyObjects = new List<object> { new object(), new object() };

            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Move", (object[] args) => moveCommandMock.Object).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.GetNearbyObjects", (object[] args) => nearbyObjects).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Check", (object[] args) => collisionCheckMock.Object).Execute();

            var command = new MoveWithCollisionCommand(movingMock.Object);

            command.Execute();

            moveCommandMock.Verify(cmd => cmd.Execute());
            collisionCheckMock.Verify(cmd => cmd.Execute(), Times.Exactly(2));
        }

        [Fact]
        public void Execute_RegistersCommandWithCorrectFactory()
        {
            var registerCommand = new RegisterMoveWithCollision();
            var mockMoving = new Mock<IMoving>().Object;

            registerCommand.Execute();

            var command = Ioc.Resolve<ICommand>("Movement.WithCollision", mockMoving);
            Assert.NotNull(command);
        }
    }
}
