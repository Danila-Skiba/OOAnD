using App;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyCollisionDataGenerator : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Collision.DataGenerator",
                (object[] args) => new CollisionDataGenerator(
                    (string)args[0],
                    (string)args[1])
            ).Execute();
        }
    }
}
