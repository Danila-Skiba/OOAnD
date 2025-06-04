namespace SpaceBattle.Lib
{
    public static class CollisionDataChecker
    {
        public static List<Point> IntersectionsFind(Polygon object1polygon, Polygon object2polygon, Point velocity)
        {
            var intersectionsPoints = new List<Point>();
            foreach (var vertex in object1polygon.Vertices)
            {
                foreach (var (start, end) in object2polygon.Edges())
                {
                    var ray = new Ray(vertex, velocity);
                    if (RayIntersectsEdge(ray, start, end, out var intersectionPoint))
                    {
                        intersectionsPoints.Add(intersectionPoint);
                    }
                }
            }

            return intersectionsPoints.Distinct().ToList();
        }

        public static bool RayIntersectsEdge(Ray ray, Point edgeStart, Point edgeEnd, out Point intersectionPoint)
        {
            intersectionPoint = new Point(0, 0);

            var rayVector = ray.RayVector;
            var edgeVector = edgeEnd - edgeStart;

            var determinant = Point.Cross(rayVector, edgeVector);
            if (Math.Abs(determinant) < 1e-6)
            {
                return false;
            }

            var edgeOffset = edgeStart - ray.StartPoint;
            var paramRay = Point.Cross(edgeOffset, edgeVector) / determinant;
            var paramEdge = Point.Cross(edgeOffset, rayVector) / determinant;

            if (paramRay >= 0 && paramEdge >= 0 && paramEdge <= 1)
            {
                intersectionPoint = ray.StartPoint + paramRay * rayVector;
                return true;
            }

            return false;
        }
    }
}
