using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencySaveCollisionDataToFileCommandTests : IDisposable
    {
        public RegisterIoCDependencySaveCollisionDataToFileCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void RegisterIoCDependencySaveCollisionDataToFileCommandRegistersCorrectlyTest()
        {
            var fileName = "test_collision.txt";
            var vectors = new List<int[]> { new[] { 1, 2, 3, 4 } };

            new RegisterIoCDependencySaveCollisionDataToFileCommand().Execute();
            var command = Ioc.Resolve<ICommand>("Collision.SaveDataToFile", fileName, vectors);

            Assert.IsType<SaveCollisionDataToFileCommand>(command);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
