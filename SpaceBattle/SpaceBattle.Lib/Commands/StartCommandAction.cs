namespace SpaceBattle.Lib
{
    public class StartCommand : ICommand
    {
        private readonly ICommand _injectableCommand;
        private readonly ICommandReceiver _receiver;
        private readonly IDictionary<string, object> _gameobject;
        private readonly string _nameCommand;
        public StartCommand(ICommand injectableCommand, ICommandReceiver receiver, IDictionary<string, object> gameobject, string name)
        {
            _gameobject = gameobject;
            _injectableCommand = injectableCommand;
            _receiver = receiver;
            _nameCommand = name;
        }

        public void Execute()
        {
            _gameobject[_nameCommand] = _injectableCommand;

            try
            {
                _receiver.Receive(_injectableCommand);
            }
            catch (Exception)
            {
                _gameobject.Remove(_nameCommand);
            }
        }
    }
}
