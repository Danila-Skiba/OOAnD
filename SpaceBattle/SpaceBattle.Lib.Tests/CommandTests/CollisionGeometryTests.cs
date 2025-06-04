namespace SpaceBattle.Lib.Tests
{
    public class CollisionGeometryTests
    {
        [Fact]
        public void PolygonConstructorCopiesVertices()
        {
            var objectVertices = new List<Point>
            {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1)
            };

            var polygon = new Polygon(objectVertices);

            Assert.Equal(objectVertices, polygon.Vertices);
            Assert.NotSame(objectVertices, polygon.Vertices);
        }

        [Fact]
        public void PolygonEdges()
        {
            var objectVertices = new List<Point>
            {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1)
            };
            var expectedEdges = new List<(Point Start, Point End)>
            {
                (objectVertices[0], objectVertices[1]),
                (objectVertices[1], objectVertices[2]),
                (objectVertices[2], objectVertices[3]),
                (objectVertices[3], objectVertices[0])
            };

            var polygon = new Polygon(objectVertices);
            var edges = polygon.Edges().ToList();

            Assert.Equal(expectedEdges, edges);
        }

        [Fact]
        public void PointCreatePointSetsCoordinates()
        {
            var point = new Point(1.5, -2.3);

            Assert.Equal(1.5, point.X);
            Assert.Equal(-2.3, point.Y);
        }

        [Fact]
        public void PointCrossCalculatesCorrectly()
        {
            var firstPoint = new Point(1, 2);
            var secondPoint = new Point(3, 4);

            var crossProduct = Point.Cross(firstPoint, secondPoint);

            Assert.Equal(-2, crossProduct);
        }

        [Fact]
        public void PointSubtractionReturnsCorrectPoint()
        {
            var firstPoint = new Point(5, 8);
            var secondPoint = new Point(2, 3);

            var result = firstPoint - secondPoint;

            Assert.Equal(3, result.X);
            Assert.Equal(5, result.Y);
        }

        [Fact]
        public void PointAdditionReturnsCorrectPoint()
        {
            var firstPoint = new Point(1, 2);
            var secondPoint = new Point(3, 4);

            var result = firstPoint + secondPoint;

            Assert.Equal(4, result.X);
            Assert.Equal(6, result.Y);
        }

        [Fact]
        public void PointScalarMultiplicationReturnsCorrectPoint()
        {
            var point = new Point(2, 3);
            const double scalar = 3;

            var result = scalar * point;

            Assert.Equal(6, result.X);
            Assert.Equal(9, result.Y);
        }

        [Fact]
        public void PointNormalizeReturnsUnitVector()
        {
            var point = new Point(3, 4);

            var normalized = point.Normalize();

            Assert.Equal(3.0 / 5, normalized.X, precision: 10);
            Assert.Equal(4.0 / 5, normalized.Y, precision: 10);
            Assert.True(Math.Abs(normalized.X * normalized.X + normalized.Y * normalized.Y - 1.0) < 1e-7);
        }

        [Fact]
        public void PointNormalizeZeroVectorReturnsSamePoint()
        {
            var point = new Point(0, 0);

            var normalized = point.Normalize();

            Assert.Equal(0, normalized.X);
            Assert.Equal(0, normalized.Y);
            Assert.Same(point, normalized);
        }

        [Fact]
        public void RayCreateConstructorSetsAndNormalizes()
        {
            var startPoint = new Point(1, 2);
            var rayVector = new Point(3, 4);

            var ray = new Ray(startPoint, rayVector);

            Assert.Equal(startPoint, ray.StartPoint);
            Assert.Equal(3.0 / 5, ray.RayVector.X, precision: 10);
            Assert.Equal(4.0 / 5, ray.RayVector.Y, precision: 10);
            Assert.True(Math.Abs(ray.RayVector.X * ray.RayVector.X + ray.RayVector.Y * ray.RayVector.Y - 1.0) < 1e-6);
        }

        [Fact]
        public void RayConstructorZeroVectorSetsNormalizedZeroVector()
        {
            var startPoint = new Point(0, 0);
            var rayVector = new Point(0, 0);

            var ray = new Ray(startPoint, rayVector);

            Assert.Equal(startPoint, ray.StartPoint);
            Assert.Equal(0, ray.RayVector.X);
            Assert.Equal(0, ray.RayVector.Y);
        }
    }
}
