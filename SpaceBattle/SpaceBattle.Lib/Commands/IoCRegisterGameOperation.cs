using App;

namespace SpaceBattle.Lib
{
    public class IoCRegisterGameOperation : ICommand
    {
        public void Execute()
        {
            var game = new Game();
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.Receiver",
                (object[] args) =>
                {
                    return game;
                }).Execute();
        }
    }
}
