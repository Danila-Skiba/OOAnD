using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class CollisionCommandTests : IDisposable
    {
        private readonly Dictionary<int, object> collisionTree = new Dictionary<int, object>(){
                {1,new Dictionary<int, object>(){
                    {1,new Dictionary<int, object>(){
                        {0, new Dictionary<int, object>(){
                            {0, new Dictionary<int, object>()}
                        }}
                    }}
                }}
            };
        public CollisionCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

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
            //Arrange
            Init_CollisionTree(collisionTree);
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
            //Act
            commandCollision.Execute();
            //Assert
            collisionHandle.Verify(c => c.Execute(), Times.Once());
        }

        [Fact]
        public void CollisionCommandFalseTest()
        {

            //Arrange
            Init_CollisionTree(collisionTree);
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
            //Act
            commandCollision.Execute();
            //Assert
            collisionHandle.Verify(c => c.Execute(), Times.Never());
        }

        [Fact]
        public void CollisionCommandExceptionRange()
        {
            //Arrange
            Init_CollisionTree(collisionTree);
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();

            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 0 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 3 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            //Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => commandCollision.Execute());
            collisionHandle.Verify(c => c.Execute(), Times.Never());

        }

        [Fact]
        public void CollisionCommandException_NonValideCollisionTree()
        {
            var newCollisionTree = new Dictionary<int, object>(){
                {1,new Dictionary<int, object>(){
                    {1,new Dictionary<int, object>(){
                        {0, new Dictionary<int, object>(){
                            {0, "end"}
                        }}
                    }}
                }}
            };
            Init_CollisionTree(newCollisionTree);

            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();

            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 3 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            commandCollision.Execute();
            //Act & Assert
            collisionHandle.Verify(c => c.Execute(), Times.Never());
        }

        [Fact]
        public void CollisionCommandExceptionRangeVelocity()
        {
            //Arrange
            Init_CollisionTree(collisionTree);
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();

            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            //Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => commandCollision.Execute());
            collisionHandle.Verify(c => c.Execute(), Times.Never());

        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }

        private static void Init_CollisionTree(IDictionary<int, object> collisionTree)
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.CollisionTree", (object[] args) => collisionTree).Execute();
        }
    }
}
