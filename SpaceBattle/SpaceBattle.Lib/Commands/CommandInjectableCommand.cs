namespace SpaceBattle.Lib
{
    public class CommandInjectableCommand : ICommand, ICommandInjectable
    {
        private ICommand? _injectableCommand;
        public void Inject(ICommand cmd)
        {
            _injectableCommand = cmd;
        }
        public void Execute()
        {
            if (_injectableCommand == null)
            {
                throw new InvalidOperationException();
            }

            _injectableCommand.Execute();
        }
    }
}
