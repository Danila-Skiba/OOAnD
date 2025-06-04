namespace SpaceBattle.Lib.Tests
{
    public class CollisionDataCheckerTests
    {
        [Fact]
        public void HasCollisionTest()
        {
            var object1polygonPoints = new List<Point> { new Point(3, 5), new Point(6, 7), new Point(8, 3), new Point(6, 2) };
            var object2polygonPoints = new List<Point> { new Point(3, 3), new Point(6, 5), new Point(5, 2) };
            var object1polygon = new Polygon(object1polygonPoints);
            var object2polygon = new Polygon(object2polygonPoints);
            var velocity = new Point(0, 1);

            var intersectionsPoints = CollisionDataChecker.IntersectionsFind(object1polygon, object2polygon, velocity);

            Assert.True(intersectionsPoints.Count > 0);
        }

        [Fact]
        public void HasNotCollisionTest()
        {
            var object1polygonPoints = new List<Point> { new Point(3, 5), new Point(6, 7), new Point(8, 3), new Point(6, 2) };
            var object2polygonPoints = new List<Point> { new Point(3, 3), new Point(6, 5), new Point(5, 2) };
            var object1polygon = new Polygon(object1polygonPoints);
            var object2polygon = new Polygon(object2polygonPoints);
            var velocity = new Point(1, 1);

            var intersectionsPoints = CollisionDataChecker.IntersectionsFind(object1polygon, object2polygon, velocity);

            Assert.Empty(intersectionsPoints);
        }
    }
}
