using App;
namespace SpaceBattle.Lib;
public class RegisterIoCDependencyCollisionDataPreparingCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Collision.DataPreparing",
            (object[] args) => new CollisionDataPreparingCommand((ICollisionDataGenerator)args[0])
        ).Execute();
    }
}
