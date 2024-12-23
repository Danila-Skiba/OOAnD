using App;

namespace SpaceBattle.Lib
{
    public class StopCommand(string objId, string cmdName) : ICommand
    {

        public void Execute()
        {
            Ioc.Resolve<ICommandInjectable>("Game.Object.GetInjectable", objId, cmdName)
                .Inject(Ioc.Resolve<ICommand>("Commands.Empty"));
            
        }
    }
}