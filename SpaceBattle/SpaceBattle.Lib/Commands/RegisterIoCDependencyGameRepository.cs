using App;

namespace SpaceBattle.Lib
{
    public class RegisterIocDependencyGameRepository : ICommand
    {
        public void Execute()
        {
            var gameItems = new Dictionary<string, object>();
            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.Item.Get",
                (object[] args) =>
                {
                    var key = (string)args[0];
                    return gameItems[(string)args[0]];
                }
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.Item.Remove",
                (object[] args) =>
                {
                    var key = (string)args[0];
                    return new Action(() =>
                    {
                        if (gameItems.ContainsKey(key))
                        {
                            gameItems.Remove(key);
                        }
                    });
                }
            ).Execute();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.Item.Add",
                (object[] args) =>
                {

                    var key = (string)args[0];
                    var value = args[1];

                    return new Action(() =>
                    {
                        if (!gameItems.ContainsKey(key))
                        {
                            gameItems.Add(key, value);
                        }
                    });
                }
            ).Execute();
        }
    }
}
