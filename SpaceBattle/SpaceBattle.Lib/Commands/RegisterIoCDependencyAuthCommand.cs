using App;

namespace SpaceBattle.Lib
{
    public class RegisterAuthDependencies : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.Auth",
                (object[] args) => new AuthCommand(
                    (string)args[0],
                    (string)args[1],
                    (string)args[2]
                )
            ).Execute();
        }
    }
}
