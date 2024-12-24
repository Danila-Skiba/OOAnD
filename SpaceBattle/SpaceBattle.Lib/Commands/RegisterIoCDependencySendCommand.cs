using App;

namespace SpaceBattle.Lib
{
    public class RegisterIoCDependencySendCommand : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.Send",
                (object[] args) => new SendCommand((ICommand)args[1], (ICommandReceiver)args[0])
            ).Execute();
        }
    }
}
