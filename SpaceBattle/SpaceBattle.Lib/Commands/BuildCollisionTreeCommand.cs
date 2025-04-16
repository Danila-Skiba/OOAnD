using App;

namespace SpaceBattle.Lib
{
    public class BuildCollisionTreeCommand : ICommand
    {
        private readonly ICollisionTreeDataProvider provider;
        private readonly string treeKey;
        public BuildCollisionTreeCommand(ICollisionTreeDataProvider provider, string treeKey)
        {
            this.provider = provider;
            this.treeKey = treeKey;
        }
        public void Execute()
        {
            var vectors = provider.GetVectors();
            var tree = new Dictionary<int, object>();
            CollisionTreeBuilder.Build(tree, vectors);
            Ioc.Resolve<App.ICommand>("IoC.Register", treeKey, (object[] args) => tree).Execute();
        }
    }
}
