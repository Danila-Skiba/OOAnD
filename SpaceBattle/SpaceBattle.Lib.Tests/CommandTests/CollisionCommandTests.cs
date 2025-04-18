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

        private readonly Dictionary<(string, string), string> collisionRules = new Dictionary<(string, string), string>
        {
            { ("Torpedo", "Ship"), "Ship" },
            { ("Ship", "Asteroid"), "Ship" }
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

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.GetType", (object[] args) =>
            {
                var obj = (IDictionary<string, object>)args[0];
                return (string)obj["Type"];
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Get.Collision.ReferenceRules", (object[] args) =>
            {
                return collisionRules;
            }).Execute();

            var registerCollDependencies = new RegiseterCollisionDependencies();

            registerCollDependencies.Execute();
        }
        [Fact]
        public void CollisionCommandTrueTest()
        {
            //Arrange
            Init_CollisionTree(collisionTree, "ShipTorpedo");
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();
            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Type"]).Returns("Torpedo");
            obj2.Setup(obj => obj["Type"]).Returns("Ship");

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
            Init_CollisionTree(collisionTree, "ShipTorpedo");
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();
            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 3 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Type"]).Returns("Torpedo");
            obj2.Setup(obj => obj["Type"]).Returns("Ship");

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);
            //Act
            commandCollision.Execute();
            //Assert
            collisionHandle.Verify(c => c.Execute(), Times.Never());
        }

        [Fact]
        public void CollisionCommandException_NonValideCollisionTree()
        {
            var newCollisionTree = new Dictionary<int, object>(){
                { 0, new Dictionary<int, object>(){
                    {0, "not a dictionary"}
                }}
            };
            Init_CollisionTree(newCollisionTree, "ShipTorpedo");

            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();

            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 3 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Type"]).Returns("Torpedo");
            obj2.Setup(obj => obj["Type"]).Returns("Ship");

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);
            //Act
            commandCollision.Execute();
            // Assert
            collisionHandle.Verify(c => c.Execute(), Times.Never());
        }

        [Fact]
        public void CollisionCommand_True_ReversePairRule()
        {
            // Arrange
            Init_CollisionTree(collisionTree, "ShipTorpedo");
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();

            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 1, 1 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Type"]).Returns("Ship");
            obj2.Setup(obj => obj["Type"]).Returns("Torpedo");

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            // Act
            commandCollision.Execute();

            // Assert
            collisionHandle.Verify(c => c.Execute(), Times.Once());
        }

        [Fact]
        public void CollisionCommand_True_NotRules()
        {
            // Arrange
            Init_CollisionTree(collisionTree, "AsteroidTorpedo");
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();

            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { 1, 1 });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { 0, 0 });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { 1, 1 });

            obj1.Setup(obj => obj["Type"]).Returns("Asteroid");
            obj2.Setup(obj => obj["Type"]).Returns("Torpedo");

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            // Act
            commandCollision.Execute();

            // Assert
            collisionHandle.Verify(c => c.Execute(), Times.Once());
        }

        [Fact]
        public void CollisionCommand_False_NullParams()
        {
            // Arrange
            Init_CollisionTree(collisionTree, "ShipTorpedo");
            var collisionHandle = new Mock<ICommand>();
            collisionHandle.Setup(c => c.Execute());

            Ioc.Resolve<App.ICommand>("IoC.Register", "Collision.Handle", (object[] args) => collisionHandle.Object).Execute();

            var obj1 = new Mock<IDictionary<string, object>>();
            var obj2 = new Mock<IDictionary<string, object>>();

            obj1.Setup(obj => obj["Position"]).Returns(new int[] { });
            obj2.Setup(obj => obj["Position"]).Returns(new int[] { });

            obj1.Setup(obj => obj["Velocity"]).Returns(new int[] { });
            obj2.Setup(obj => obj["Velocity"]).Returns(new int[] { });

            obj1.Setup(obj => obj["Type"]).Returns("Torpedo");
            obj2.Setup(obj => obj["Type"]).Returns("Ship");

            var commandCollision = new CollisionCommand(obj1.Object, obj2.Object);

            // Act
            commandCollision.Execute();

            // Assert
            collisionHandle.Verify(c => c.Execute(), Times.Never());
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }

        private static void Init_CollisionTree(IDictionary<int, object> collisionTree, string treeKey)
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", $"Game.CollisionTree.{treeKey}", (object[] args) => collisionTree).Execute();
        }
    }
}
