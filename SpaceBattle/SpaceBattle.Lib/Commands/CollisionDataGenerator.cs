using App;

namespace SpaceBattle.Lib
{
    public class CollisionDataGenerator : ICollisionDataGenerator
    {
        public string object1 { get; }
        public string object2 { get; }
        private readonly Random random;

        public CollisionDataGenerator(string object1, string object2)
        {
            this.object1 = object1;
            this.object2 = object2;
            random = new Random();
        }

        public IList<int[]> GenerateCollisionData()
        {
            var objectsDict = Ioc.Resolve<IDictionary<string, Polygon>>("Collision.ObjectsDictionary");
            var dataSize = Ioc.Resolve<int>("Data.Size");
            var fieldSize = Ioc.Resolve<int>("Field.Size");

            var object1Shape = objectsDict[object1];
            var object2Shape = objectsDict[object2];
            var collisionVectors = new List<int[]>();
            var maxtriesCount = Ioc.Resolve<int>("CollisionGenerator.MaxTries");
            var tryCount = 0;

            while (collisionVectors.Count < dataSize && tryCount < maxtriesCount)
            {
                var offsetPoint = GeneratePoint(fieldSize);
                var velocity = GeneratePoint(fieldSize / 5);
                var objectVertices = object1Shape.Edges().SelectMany(e => new[] { e.Start, e.End }).Distinct().Select(p => p + offsetPoint).ToList();

                var object1Shapeshifted = new Polygon(objectVertices);
                var collisions = CollisionDataChecker.IntersectionsFind(object1Shapeshifted, object2Shape, velocity);

                if (collisions.Count > 0)
                {
                    var intersectionPoint = collisions[0];
                    collisionVectors.Add(new int[]
                    {
                        (int)intersectionPoint.X,
                        (int)intersectionPoint.Y,
                        (int)velocity.X,
                        (int)velocity.Y
                    });
                }

                tryCount++;
            }

            return collisionVectors;
        }
        public Point GeneratePoint(int range)
        {
            return new Point(
                random.Next(0, range),
                random.Next(0, range)
            );
        }
    }
}
