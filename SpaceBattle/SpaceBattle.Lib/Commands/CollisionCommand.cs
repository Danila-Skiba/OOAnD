using App;

namespace SpaceBattle.Lib
{
    public class CollisionCommand : ICommand
    {
        private readonly object _obj1;
        private readonly object _obj2;

        public CollisionCommand(object obj1, object obj2)
        {
            _obj1 = obj1;
            _obj2 = obj2;
        }

        public void Execute()
        {
            var values = Ioc.Resolve<(int[] branch, string treeKey)>("Get.Collision.State", _obj1, _obj2);

            var collisionTree = Ioc.Resolve<IDictionary<int, object>>($"Game.CollisionTree.{values.treeKey}");

            if (CheckCollision(collisionTree, values.branch))
            {
                Ioc.Resolve<ICommand>("Collision.Handle", _obj1, _obj2).Execute();
            }
        }

        private static bool CheckCollision(IDictionary<int, object> collTree, int[] parameters)
        {
            object current = collTree;
            if (parameters.Length == 0)
            {
                return false;
            }

            return parameters.All(param =>
            {
                if (current is IDictionary<int, object> dict && dict.TryGetValue(param, out var next))
                {
                    current = next;
                    return true;
                }

                return false;
            });
        }
    }
}
