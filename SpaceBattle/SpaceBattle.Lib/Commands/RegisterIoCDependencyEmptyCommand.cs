using App;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencyEmptyCommand : ICommand
    {
        public void Execute()
        {
            var emptyCommand = new EmptyCommand();
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.Empty",
                (object[] args) => emptyCommand
            ).Execute();
        }
    }
}
