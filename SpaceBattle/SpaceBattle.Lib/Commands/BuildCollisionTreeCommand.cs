using App;

namespace SpaceBattle.Lib
{
    public class BuildCollisionTreeCommand : ICommand
    {
        private readonly ICollisionTreeDataProvider _provider;
        public BuildCollisionTreeCommand(ICollisionTreeDataProvider provider)
        {
            _provider = provider;
        }
        public void Execute()
        {
            var vectors = _provider.GetCollisionVectors();
            var tree = new Dictionary<int, object>();

            vectors.ToList().ForEach(vector =>
            {
                if (vector == null)
                {
                    return;
                }

                var current = tree;

                vector.Take(vector.Length - 1).ToList().ForEach(param =>
                {
                    if (!current.ContainsKey(param))
                    {
                        current[param] = new Dictionary<int, object>();
                    }
                    current = (Dictionary<int, object>)current[param];
                });

                var lastParam = vector.Last();
                if (!current.ContainsKey(lastParam))
                {
                    current[lastParam] = new Dictionary<int, object>();
                }
            });
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Struct.CollisionTree", (object[] args) => tree).Execute();
        }
    }
}
