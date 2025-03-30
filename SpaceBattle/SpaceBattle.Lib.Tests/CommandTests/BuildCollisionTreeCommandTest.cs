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
        public void BuildCollisionTreeWithLength4()
        {
            var providerMock = new Mock<ICollisionTreeDataProvider>();
            providerMock.Setup(p => p.GetCollisionVectors()).Returns(new List<int[]>
            {
                new [] { 1, 1, -1, -1},
                new [] { 1, 1, 0, 0},
                new [] { 2, 2, 1, 1}
            });

            var buildcommand = new BuildCollisionTreeCommand(providerMock.Object);
            buildcommand.Execute();

            var tree = Ioc.Resolve<IDictionary<int, object>>("Game.Struct.CollisionTree");
            Assert.NotNull(tree);

            var level1 = (IDictionary<int, object>)tree[1];
            var level2 = (IDictionary<int, object>)level1[1];
            var level3 = (IDictionary<int, object>)level2[-1];
            var level4 = (IDictionary<int, object>)level3[-1];
            Assert.IsType<Dictionary<int, object>>(level4);

            level3 = (IDictionary<int, object>)level2[0];
            level4 = (IDictionary<int, object>)level3[0];
            Assert.IsType<Dictionary<int, object>>(level4);

            level1 = (IDictionary<int, object>)tree[2];
            level2 = (IDictionary<int, object>)level1[2];
            level3 = (IDictionary<int, object>)level2[1];
            level4 = (IDictionary<int, object>)level3[1];
            Assert.IsType<Dictionary<int, object>>(level4);
        }

        [Fact]
        public void BuildCollisionTreeCommandWithInvalidVectors()
        {
            var providerMock = new Mock<ICollisionTreeDataProvider>();
            providerMock.Setup(p => p.GetCollisionVectors()).Returns(new List<int[]>
            {
                null,
                new [] { 1, 1, -1, -1}
            });

            var buildcommand = new BuildCollisionTreeCommand(providerMock.Object);
            buildcommand.Execute();

            var tree = Ioc.Resolve<IDictionary<int, object>>("Game.Struct.CollisionTree");
            Assert.NotNull(tree);

            var level1 = (IDictionary<int, object>)tree[1];
            var level2 = (IDictionary<int, object>)level1[1];
            var level3 = (IDictionary<int, object>)level2[-1];
            var level4 = (IDictionary<int, object>)level3[-1];
            Assert.IsType<Dictionary<int, object>>(level4);
        }

        [Fact]
        public void BuildCollisionTreeCommandEmptyVectors()
        {
            var providerMock = new Mock<ICollisionTreeDataProvider>();
            providerMock.Setup(p => p.GetCollisionVectors()).Returns(new List<int[]>());

            var buildCommand = new BuildCollisionTreeCommand(providerMock.Object);
            buildCommand.Execute();

            var tree = Ioc.Resolve<IDictionary<int, object>>("Game.Struct.CollisionTree");
            Assert.NotNull(tree);
            Assert.Empty(tree);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
