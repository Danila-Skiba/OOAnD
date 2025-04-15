namespace SpaceBattle.Lib
{
    public class CollisionTreeDataProvider : ICollisionTreeDataProvider
    {
        private readonly List<int[]> vectors;

        public CollisionTreeDataProvider(List<int[]> vectors)
        {
            this.vectors = vectors;
        }
        public List<int[]> GetVectors()
        {
            return vectors;
        }
    }
}
