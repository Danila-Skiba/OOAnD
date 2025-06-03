using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class CollisionDataPreparingCommandTests : IDisposable
    {
        public CollisionDataPreparingCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void CollisionDataPreparingGeneratesAndSavesDataTest()
        {
            var mockGenerator = new Mock<ICollisionDataGenerator>();
            mockGenerator.Setup(g => g.GenerateCollisionData()).Returns(new List<int[]> { new[] { 1, 2, 3, 4 }, new[] { 1, 2, 0, 0 } });
            mockGenerator.SetupGet(g => g.object1).Returns("ship");
            mockGenerator.SetupGet(g => g.object2).Returns("asteroid");
            var mockSave = new Mock<ICommand>();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Collision.SaveDataToFile",
                (object[] args) =>
                {
                    Assert.Equal("ship_asteroid.txt", args[0]);
                    Assert.Equal(2, ((IList<int[]>)args[1]).Count);
                    Assert.Equal(new[] { 1, 2, 3, 4 }, ((IList<int[]>)args[1])[0]);
                    Assert.Equal(new[] { 1, 2, 0, 0 }, ((IList<int[]>)args[1])[1]);
                    return mockSave.Object;
                }
            ).Execute();

            new RegisterIoCDependencyCollisionDataPreparingCommand().Execute();

            var command = Ioc.Resolve<ICommand>("Collision.DataPreparing", mockGenerator.Object);
            command.Execute();

            mockGenerator.Verify(g => g.GenerateCollisionData(), Times.Once());
            mockGenerator.VerifyGet(g => g.object1, Times.Once());
            mockGenerator.VerifyGet(g => g.object2, Times.Once());
            mockSave.Verify(c => c.Execute(), Times.Once());
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
