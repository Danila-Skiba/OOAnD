using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests.CommandTests
{
    public class BuildCollisionTreeCommandTest : IDisposable
    {
        public BuildCollisionTreeCommandTest()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void BuildCollisionTreeFromFile()
        {
            var treeKey = "Game.CollisionTree.Object1";
            var filePath = "../../../testTree.txt";

            Ioc.Resolve<App.ICommand>("IoC.Register", "CollisionDataProvider.FromFile", (object[] args) => new FileCollisionTreeDataProvider((string)args[0])).Execute();
            var provider = Ioc.Resolve<ICollisionTreeDataProvider>("CollisionDataProvider.FromFile", filePath);
            var vectors = provider.GetVectors();

            Assert.NotEmpty(vectors);
            Assert.Contains(vectors, v => v.Length > 0 && v[0] == 1);

            var providerMock = new Mock<ICollisionTreeDataProvider>();
            providerMock.Setup(p => p.GetVectors()).Returns(vectors);

            var buildCommand = new BuildCollisionTreeCommand(providerMock.Object, treeKey);
            buildCommand.Execute();

            var tree = Ioc.Resolve<Dictionary<int, object>>(treeKey);
            Assert.NotNull(tree);

            Assert.Contains(1, tree.Keys);
            var level1_1 = (Dictionary<int, object>)tree[1];
            Assert.Contains(2, level1_1.Keys);
            var level2_1 = (Dictionary<int, object>)level1_1[2];
            Assert.Contains(3, level2_1.Keys);
            var level3_1 = (Dictionary<int, object>)level2_1[3];
            Assert.Contains(4, level3_1.Keys);

            var level2_2 = (Dictionary<int, object>)level1_1[2];
            Assert.Contains(0, level2_2.Keys);
            var level3_2 = (Dictionary<int, object>)level2_2[0];
            Assert.Contains(0, level3_2.Keys);

            Assert.Contains(2, tree.Keys);
            var level1_3 = (Dictionary<int, object>)tree[2];
            Assert.Contains(2, level1_3.Keys);
            var level2_3 = (Dictionary<int, object>)level1_3[2];
            Assert.Contains(1, level2_3.Keys);
            var level3_3 = (Dictionary<int, object>)level2_3[1];
            Assert.Contains(1, level3_3.Keys);
        }

        [Fact]
        public void BuildCollisionTreeCommandFailsToReadFile()
        {
            var treeKey = "Game.CollisionTree.Object2";
            var invalidFilePath = "nonexistent_file.txt";

            Ioc.Resolve<App.ICommand>("IoC.Register", "CollisionDataProvider.FromFile", (object[] args) => new FileCollisionTreeDataProvider((string)args[0])).Execute();
            var provider = Ioc.Resolve<ICollisionTreeDataProvider>("CollisionDataProvider.FromFile", invalidFilePath);

            var buildCommand = new BuildCollisionTreeCommand(provider, treeKey);

            var exception = Assert.Throws<FileNotFoundException>(() => buildCommand.Execute());
            Assert.Contains("nonexistent_file.txt", exception.Message);
        }

        [Fact]
        public void CollisionTreeDataProviderReturnsGivenVectors()
        {
            var filePath = "../../../testTree.txt";

            var inputVectors = File.ReadLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(int.Parse)
                                    .ToArray())
                .ToList();

            Ioc.Resolve<App.ICommand>("IoC.Register", "CollisionDataProvider.FromMemory", (object[] args) => new CollisionTreeDataProvider((List<int[]>)args[0])).Execute();
            var provider = Ioc.Resolve<ICollisionTreeDataProvider>("CollisionDataProvider.FromMemory", inputVectors);
            var result = provider.GetVectors();

            Assert.Equal(inputVectors, result);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
