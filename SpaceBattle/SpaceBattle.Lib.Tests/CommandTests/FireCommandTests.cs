﻿using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class FireCommandTests : IDisposable
    {
        public FireCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Execute_ShouldRegisterFireCommandDependency()
        {
            var position = new Vector(new[] { 0, 0 });
            var fireDirection = new Vector(new[] { 2, 1 });
            var speed = 2.0;
            var velocity = new Vector(new[] { (int)(2 * speed), (int)(1 * speed) });
            var cmd = new Mock<ICommand>();

            var weaponMock = new Mock<IMoving>();
            weaponMock.SetupGet(a => a.Position).Returns(position);
            weaponMock.SetupGet(a => a.Velocity).Returns(velocity);
            weaponMock.SetupProperty(w => w.Position);

            var receiverMock = new Mock<ICommandReceiver>();
            var gameItems = new Dictionary<string, object>();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Weapon.Create", (object[] args) =>
            {
                return new Dictionary<string, object> { { "Id", (string)args[0] } };
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Adapters.IMovingObject", (object[] args) =>
            {
                return weaponMock.Object;
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Weapon.Setup", (object[] args) =>
            {
                var weapon = (IMoving)args[0];
                var pos = (Vector)args[1];
                weapon.Position = pos;
                return cmd.Object;
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Receiver", (object[] args) =>
            {
                return receiverMock.Object;
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Item.Add", (object[] args) =>
            {
                var id = (string)args[0];
                var item = args[1];
                gameItems[id] = item;
                return cmd.Object;
            }).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Item.Get", (object[] args) => gameItems[(string)args[0]]).Execute();

            new RegisterIoCDependencyMoveCommand().Execute();
            new RegisterFireDependencies().Execute();
            new RegisterIocDependencyActionsStart().Execute();

            var fireCommand = Ioc.Resolve<ICommand>("Commands.Fire", position, fireDirection, speed);
            fireCommand.Execute();

            Assert.IsType<FireCommand>(fireCommand);
            receiverMock.Verify(r => r.Receive(It.IsAny<ICommand>()), Times.Once());
        }

        [Fact]
        public void Execute_NotShouldRegisterFireCommandDependency()
        {
            Assert.ThrowsAny<Exception>(() => Ioc.Resolve<ICommand>("Commands.Fire"));
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
