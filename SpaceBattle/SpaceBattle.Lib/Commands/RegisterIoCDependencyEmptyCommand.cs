using App;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyEmptyCommand : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.Empty",
                (object[] args) => new EmptyCommand()
            ).Execute();
        }
    }
}
