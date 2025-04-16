using App;

namespace SpaceBattle.Lib
{
    public class IoCRegisterGameOperation : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Receiver", (object[] args) =>
            {
                return new GameReceiver();
            }).Execute();
        }
    }
}
