using App;

namespace SpaceBattle.Lib
{
    public class RegisterAdapterCommand<TInterface, TCommand> : ICommand
        where TCommand : class
    {
        private readonly string _key;
        public RegisterAdapterCommand(string key)
        {
            _key = key;
        }

        public void Execute()
        {
            Ioc.Resolve<App.ICommand>(
                "IoC.Register", _key,
                (object[] args) =>
                {
                    var objDict = (IDictionary<string, object>)args[0];

                    var service = (TInterface)Ioc.Resolve<object>(
                        "Game.Adapter.Create", objDict, typeof(TInterface));

                    return Activator.CreateInstance(typeof(TCommand), service)!;
                })
            .Execute();
        }
    }
}
