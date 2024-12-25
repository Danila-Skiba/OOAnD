using App;

namespace SpaceBattle.Lib
{
    public class StopCommand: App.ICommand
    {
        private readonly string _objId;
        private readonly string _cmdName;
        public StopCommand(string objId, string cmdName)
        {
            _cmdName = cmdName;
            _objId = objId;
            
        }

        public void Execute()
        {
            Ioc.Resolve<ICommandInjectable>("Game.Object.GetInjectable", _objId, _cmdName)
                .Inject(Ioc.Resolve<ICommand>("Commands.Empty"));
   
        }
    }
}
