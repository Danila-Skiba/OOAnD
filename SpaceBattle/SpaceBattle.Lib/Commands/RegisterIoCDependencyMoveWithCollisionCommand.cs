using App;

namespace SpaceBattle.Lib
{
    public class RegisterMoveWithCollision : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Movement.WithCollision",
                (object[] args) => new MoveWithCollisionCommand((IMoving)args[0])).Execute();
        }
    }
}
