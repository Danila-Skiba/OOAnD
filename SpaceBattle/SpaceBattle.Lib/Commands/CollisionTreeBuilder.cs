namespace SpaceBattle.Lib
{
    public static class CollisionTreeBuilder
    {
        public static void Build(Dictionary<int, object> tree, List<int[]> vectors)
        {
            vectors.ForEach(vector =>
            {
                var current = tree;
                vector.ToList().ForEach(param =>
                {
                    current.TryAdd(param, new Dictionary<int, object>());
                    current = (Dictionary<int, object>)current[param];
                });
            });
        }
    }
}
