using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class SaveCollisionDataToFileCommandTests : IDisposable
    {
        public SaveCollisionDataToFileCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void SaveCollisionDataToFileTest()
        {
            var fileName = "test_collision.txt";
            var basePath = Path.GetTempPath();
            var fullPath = Path.Combine(basePath, fileName);
            var vectors = new List<int[]> { new[] { 1, 2, 3, 4 }, new[] { 1, 2, 0, 0 } };

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Data.FilePath",
                (object[] args) => basePath
            ).Execute();

            new RegisterIoCDependencySaveCollisionDataToFileCommand().Execute();

            var command = Ioc.Resolve<ICommand>("Collision.SaveDataToFile", fileName, vectors);
            command.Execute();

            var lines = File.Exists(fullPath) ? File.ReadAllLines(fullPath) : new string[0];
            Assert.Equal(2, lines.Length);
            Assert.Equal("1 2 3 4", lines[0]);
            Assert.Equal("1 2 0 0", lines[1]);

            File.Delete(fullPath);
        }

        [Fact]
        public void SaveCollisionDataToFileWithInvalidVectorTest()
        {
            var fileName = "test_invalid.txt";
            var basePath = Path.GetTempPath();
            var fullPath = Path.Combine(basePath, fileName);
            var vectors = new List<int[]> { new[] { 1, 2, 3 }, new[] { 1, 2, 0, 0 } };

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Data.FilePath",
                (object[] args) => basePath
            ).Execute();

            new RegisterIoCDependencySaveCollisionDataToFileCommand().Execute();

            var command = Ioc.Resolve<ICommand>("Collision.SaveDataToFile", fileName, vectors);
            command.Execute();

            var lines = File.Exists(fullPath) ? File.ReadAllLines(fullPath) : new string[0];
            Assert.Equal(2, lines.Length);
            Assert.Equal("1 2 3", lines[0]);
            Assert.Equal("1 2 0 0", lines[1]);

            File.Delete(fullPath);
        }

        [Fact]
        public void SaveCollisionDataToFileEmptyVectorsTest()
        {
            var fileName = "test_empty.txt";
            var basePath = Path.GetTempPath();
            var fullPath = Path.Combine(basePath, fileName);
            var vectors = new List<int[]>();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Data.FilePath",
                (object[] args) => basePath
            ).Execute();

            new RegisterIoCDependencySaveCollisionDataToFileCommand().Execute();

            var command = Ioc.Resolve<ICommand>("Collision.SaveDataToFile", fileName, vectors);
            command.Execute();

            var lines = File.Exists(fullPath) ? File.ReadAllLines(fullPath) : new string[0];
            Assert.Empty(lines);

            File.Delete(fullPath);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
