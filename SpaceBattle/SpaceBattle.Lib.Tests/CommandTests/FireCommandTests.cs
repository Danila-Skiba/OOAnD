using App;
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
            var fireableMock = new Mock<IFireable>();
            var position = new Vector(new[] { 0, 0 });
            var fireDirection = new Vector(new[] { 2, 1 });
            fireableMock.Setup(f => f.Position).Returns(position);
            fireableMock.Setup(f => f.FireDirection).Returns(fireDirection);

            var weaponMock = new Mock<IWeapon>();
            weaponMock.Setup(w => w.Position).Returns(position);
            weaponMock.Setup(w => w.Velocity).Returns(fireDirection);

            Ioc.Resolve<App.ICommand>("IoC.Register", "Weapon.Create", (object[] args) =>
            {
                return weaponMock.Object;
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Adapters.IFireableObject", (object[] args) =>
            {
                return fireableMock.Object;
            }).Execute();

            new RegisterIocDependencyGameRepository().Execute();

            new RegisterFireDependencies().Execute();

            var fireCommand = Ioc.Resolve<ICommand>("Commands.Fire", fireableMock.Object);
            fireCommand.Execute();

            Assert.IsType<FireCommand>(fireCommand);
            var weaponId = ((FireCommand)fireCommand).GetLastWeaponId();
            var addedItem = Ioc.Resolve<object>("Game.Item.Get", weaponId);
            Assert.Equal(weaponMock.Object, addedItem);
        }

        [Fact]
        public void Execute_NotShouldRegisterFireCommandDependency()
        {
            var fireableMock = new Mock<IFireable>();

            Assert.ThrowsAny<Exception>(() => Ioc.Resolve<ICommand>("Commands.Fire", fireableMock.Object));
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
