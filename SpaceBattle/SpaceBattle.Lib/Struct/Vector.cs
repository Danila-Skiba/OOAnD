namespace SpaceBattle.Lib;
public class Vector
{
    private readonly int[] coordinates;

    public Vector(int[] coord)
    {
        coordinates = coord;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector vector && coordinates.SequenceEqual(vector.coordinates);
    }

    public override int GetHashCode()
    {
        return System.HashCode.Combine(coordinates);
    }

    public static Vector operator +(Vector vector1, Vector vector2)
    {
        if (vector1.coordinates.Length != vector2.coordinates.Length)
        {
            throw new InvalidOperationException();
        }

        return new Vector(vector1.coordinates.Zip(vector2.coordinates, (v1, v2) => v1 + v2).ToArray());
    }

    public static bool operator ==(Vector vector1, Vector vector2)
    {
        return vector1.Equals(vector2);
    }

    public static bool operator !=(Vector vector1, Vector vector2)
    {
        return !(vector1 == vector2);
    }
}
