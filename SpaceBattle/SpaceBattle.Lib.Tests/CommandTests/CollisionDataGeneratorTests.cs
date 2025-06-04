using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class CollisionDataGeneratorTests : IDisposable
    {
        public CollisionDataGeneratorTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void GenerateCollisionDataWithCollisionsReturnsVectors()
        {
            var object1 = "ship";
            var object2 = "asteroid";
            var objectShapes = new Dictionary<string, Polygon>
            {
                { object1, new Polygon(new List<Point> { new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(0, 1) }) },
                { object2, new Polygon(new List<Point> { new Point(0, 2), new Point(2, 2) }) }
            };

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Collision.ObjectsDictionary",
                (object[] args) => objectShapes
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Data.Size",
                (object[] args) => (object)1
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Field.Size",
                (object[] args) => (object)100
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "CollisionGenerator.MaxTries",
                (object[] args) => (object)1000
            ).Execute();

            var generator = new CollisionDataGenerator(object1, object2);

            var collisionVectors = generator.GenerateCollisionData();

            Assert.Single(collisionVectors);
            Assert.Equal(4, collisionVectors[0].Length);
            Assert.InRange(collisionVectors[0][2], -20, 20);
            Assert.InRange(collisionVectors[0][3], -20, 20);
        }

        [Fact]
        public void GenerateCollisionDataZeroDataSizeReturnsEmptyList()
        {
            var object1 = "ship";
            var object2 = "asteroid";
            var objectShapes = new Dictionary<string, Polygon>
            {
                { object1, new Polygon(new List<Point>()) },
                { object2, new Polygon(new List<Point>()) }
            };

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Collision.ObjectsDictionary",
                (object[] args) => objectShapes
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Data.Size",
                (object[] args) => (object)10
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Field.Size",
                (object[] args) => (object)50
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "CollisionGenerator.MaxTries",
                (object[] args) => (object)50
            ).Execute();

            var generator = new CollisionDataGenerator(object1, object2);

            var collisionVectors = generator.GenerateCollisionData();

            Assert.Empty(collisionVectors);
        }

        [Fact]
        public void GeneratePointReturnsPointInRange()
        {
            var generator = new CollisionDataGenerator("ship", "asteroid");
            var range = 5;

            var point = generator.GeneratePoint(range);

            Assert.InRange(point.X, -range, range);
            Assert.InRange(point.Y, -range, range);
        }

        [Fact]
        public void GenerateCollisionDataEmptyPolygonsReturnsEmptyList()
        {
            var object1 = "ship";
            var object2 = "asteroid";
            var objectShapes = new Dictionary<string, Polygon>
            {
                { object1, new Polygon(new List<Point>()) },
                { object2, new Polygon(new List<Point>()) }
            };

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Collision.ObjectsDictionary",
                (object[] args) => objectShapes
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Data.Size",
                (object[] args) => (object)10
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Field.Size",
                (object[] args) => (object)50
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "CollisionGenerator.MaxTries",
                (object[] args) => (object)50
            ).Execute();

            var generator = new CollisionDataGenerator(object1, object2);

            var collisionVectors = generator.GenerateCollisionData();

            Assert.Empty(collisionVectors);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
