using App;

namespace SpaceBattle.Lib;
public class RegisterIoCDependencySaveCollisionDataToFileCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Collision.SaveDataToFile",
            (object[] args) => new SaveCollisionDataToFileCommand((string)args[0], (IList<int[]>)args[1])
        ).Execute();
    }
}
