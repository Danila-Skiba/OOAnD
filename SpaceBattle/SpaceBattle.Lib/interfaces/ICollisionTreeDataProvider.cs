namespace SpaceBattle.Lib
{
    public interface ICollisionTreeDataProvider
    {
        IEnumerable<int[]?> GetCollisionVectors();
    }
}
