using App;
using System.Linq;

namespace SpaceBattle.Lib.Commands
{
    public class MacroCommand : ICommand
    {
        private readonly ICommand[] _commands;

        public MacroCommand(ICommand[] commands)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public void Execute()
        {
            _commands.ToList().ForEach(command => command.Execute());
        }
    }
}
