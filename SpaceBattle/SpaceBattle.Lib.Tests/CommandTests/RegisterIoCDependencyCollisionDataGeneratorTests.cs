using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyCollisionDataGeneratorTests : IDisposable
    {
        public RegisterIoCDependencyCollisionDataGeneratorTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }
        [Fact]
        public void RegisterIoCDependencyCollisionDataGeneratorRegistersCorrectlyTest()
        {
            new RegisterIoCDependencyCollisionDataGenerator().Execute();
            var generator = Ioc.Resolve<CollisionDataGenerator>("Collision.DataGenerator", "ship", "asteroid");

            Assert.IsType<CollisionDataGenerator>(generator);
            Assert.Equal("ship", generator.object1);
            Assert.Equal("asteroid", generator.object2);
        }

        [Fact]
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
