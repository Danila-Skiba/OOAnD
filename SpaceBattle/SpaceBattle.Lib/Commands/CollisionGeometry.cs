namespace SpaceBattle.Lib
{
    public class Polygon
    {
        public List<Point> Vertices { get; }
        public Polygon(IEnumerable<Point> objectVertices)
        {
            Vertices = objectVertices.ToList();
        }

        public IEnumerable<(Point Start, Point End)> Edges()
        {
            return Vertices.Select((vertex, index) => (vertex, Vertices[(index + 1) % Vertices.Count]));
        }
    }

    public class Point
    {
        public double X { get; }
        public double Y { get; }
        public Point(double coordX, double coordY)
        {
            X = coordX;
            Y = coordY;
        }

        public static double Cross(Point firstPoint, Point secondPoint)
        {
            return firstPoint.X * secondPoint.Y - firstPoint.Y * secondPoint.X;
        }

        public static Point operator -(Point firstPoint, Point secondPoint)
        {
            return new Point(firstPoint.X - secondPoint.X, firstPoint.Y - secondPoint.Y);
        }

        public static Point operator +(Point firstPoint, Point secondPoint)
        {
            return new Point(firstPoint.X + secondPoint.X, firstPoint.Y + secondPoint.Y);
        }

        public static Point operator *(double scalar, Point point)
        {
            return new Point(point.X * scalar, point.Y * scalar);
        }

        public Point Normalize()
        {
            var length = Math.Sqrt(X * X + Y * Y);
            return length > 1e-7 ? new Point(X / length, Y / length) : this;
        }
    }

    public class Ray
    {
        public Point StartPoint { get; }
        public Point RayVector { get; }
        public Ray(Point startPoint, Point rayVector)
        {
            StartPoint = startPoint;
            RayVector = rayVector.Normalize();
        }
    }
}
