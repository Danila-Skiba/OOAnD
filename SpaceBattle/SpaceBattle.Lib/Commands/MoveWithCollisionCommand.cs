using App;

namespace SpaceBattle.Lib
{
    public class MoveWithCollisionCommand : ICommand
    {
        private readonly IMoving _moving;

        public MoveWithCollisionCommand(IMoving moving) => _moving = moving;

        public void Execute()
        {
            Ioc.Resolve<ICommand>("Commands.Move", _moving).Execute();

            var nearbyObjects = Ioc.Resolve<IEnumerable<object>>("Collision.GetNearbyObjects", _moving.Position)
                .Where(obj => obj != _moving);

            foreach (var obj in nearbyObjects)
            {
                Ioc.Resolve<ICommand>("Collision.Check", _moving, obj).Execute();
            }
        }
    }
}
