using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class CollisionCommandTests : IDisposable
    {
        public CollisionCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

            var collisionTree = new Dictionary<int, object>(){
                {1,new Dictionary<int, object>(){
                    {1,new Dictionary<int, object>(){
                        {0, new Dictionary<int, object>(){
                            {0, new Dictionary<int, object>()}
                        }}
                    }}
                }}
            };
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.CollisionTree", (object[] args) => collisionTree).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.GetPosition", (object[] args) =>
            {
                var obj = (IDictionary<string, object>)args[0];
                return (int[])obj["Position"];
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.GetVelocity", (object[] args) =>
            {
                var obj = (IDictionary<string, object>)args[0];
                return (int[])obj["Velocity"];
            }).Execute();
        }
        [Fact]
        public void CollisionCommandTrueTest()
        {
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();
            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            commandCollision.Execute();

            collisionHandle.Verify(c => c.Execute(), Times.Once());
        }

        [Fact]
        public void CollisionCommandFalseTest()
        {
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();
            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 3 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            commandCollision.Execute();

            collisionHandle.Verify(c => c.Execute(), Times.Never());
        }
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
