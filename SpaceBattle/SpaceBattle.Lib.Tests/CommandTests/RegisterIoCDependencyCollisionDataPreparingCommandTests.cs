using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyCollisionDataPreparingCommandTests : IDisposable
    {
        public RegisterIoCDependencyCollisionDataPreparingCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void RegisterIoCDependencyCollisionDataPreparingCommandRegistersCorrectlyTest()
        {
            var mockGenerator = new Mock<ICollisionDataGenerator>();

            new RegisterIoCDependencyCollisionDataPreparingCommand().Execute();
            var command = Ioc.Resolve<ICommand>("Collision.DataPreparing", mockGenerator.Object);

            Assert.IsType<CollisionDataPreparingCommand>(command);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
