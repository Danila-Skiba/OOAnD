using App;

namespace SpaceBattle.Lib
{
    public class BuildCollisionTreeCommand : ICommand
    {
        private readonly ICollisionTreeDataProvider provider;

        public BuildCollisionTreeCommand(ICollisionTreeDataProvider provider)
        {
            this.provider = provider;
        }
        public void Execute()
        {
            var vectors = provider.GetVectors();
            var tree = Ioc.Resolve<Dictionary<int, object>>("Game.Struct.CollisionTree");
            CollisionTreeBuilder.Build(tree, vectors);
        }
    }
}
