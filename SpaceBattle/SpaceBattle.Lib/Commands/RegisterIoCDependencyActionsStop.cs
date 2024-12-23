using App;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyActionsStop : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Actions.Stop",
                (object[] args) => new StopCommand((string)args[0], (string)args[1])
            ).Execute();
        }
    }
}
