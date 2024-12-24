using App;

namespace SpaceBattle.Lib
{
    public class RegisterIocDependencyActionsStart : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Actions.Start",
                (object[] args) =>
                {
                    var order = (IDictionary<string, object>)args[0];
                    var cmd = (ICommand)order["Cmd"];
                    var receiver = (ICommandReceiver)order["Receiver"];
                    var name_cmd = (string)order["Name"];
                    var gameobject = (IDictionary<string, object>)order["Object"];

                    return new StartCommand(cmd, receiver, gameobject, name_cmd);
                }
            ).Execute();
        }
    }
}
