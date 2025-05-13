namespace SpaceBattle.Lib;
public interface ICollisionDataGenerator
{
    string object1 { get; }
    string object2 { get; }
    IList<int[]> GenerateCollisionData();
}
