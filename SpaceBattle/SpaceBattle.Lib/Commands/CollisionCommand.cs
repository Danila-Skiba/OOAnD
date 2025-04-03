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
            var position1 = Ioc.Resolve<int[]>("Game.GetPosition", _obj1);
            var position2 = Ioc.Resolve<int[]>("Game.GetPosition", _obj2);

            var velocity1 = Ioc.Resolve<int[]>("Game.GetVelocity", _obj1);
            var velocity2 = Ioc.Resolve<int[]>("Game.GetVelocity", _obj2);

            var dPositions = position2.ToList().Select((value, index) => value - position1[index]);
            var dVelocities = velocity2.ToList().Select((value, index) => value - velocity1[index]);

            var branch = dPositions.Concat(dVelocities).ToArray();

            var collisionTree = Ioc.Resolve<IDictionary<int, object>>("Game.CollisionTree");

            var collision = CheckCollision(collisionTree, branch);

            if (collision)
            {
                Ioc.Resolve<ICommand>("Collision.Handle", _obj1, _obj2).Execute();
            }
        }

        private static bool CheckCollision(IDictionary<int, object> collTree, int[] parameters)
        {
            object current = collTree;
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
