using App;

namespace SpaceBattle.Lib
{
    public class RegisterCollisionCommandWithGrid : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Collision.WithGrid",
                (object[] args) => new CollisionCommandWithGrid((IMoving)args[0])).Execute();
        }
    }
}