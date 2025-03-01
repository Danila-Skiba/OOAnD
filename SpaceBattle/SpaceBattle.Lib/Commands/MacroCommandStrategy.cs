﻿using App;

namespace SpaceBattle.Lib
{
    public class CreateMacroCommandStrategy
    {
        private readonly string commandSpec;

        public CreateMacroCommandStrategy(string commandSpec)
        {
            this.commandSpec = commandSpec;
        }

        public ICommand Resolve(object[] args)
        {
            var commandNames = Ioc.Resolve<IEnumerable<string>>("Specs." + commandSpec);

            var commands = commandNames.Select((name, index) =>
            {
                var commandArgs = new object[] { args[index] };
                var command = Ioc.Resolve<ICommand>(name, commandArgs);

                if (commandArgs[0] == null)
                {
                    throw new InvalidOperationException($"Команда '{name}' принимает аргумент null.");
                }

                return command;
            }).ToArray();

            var macroCmd = Ioc.Resolve<ICommand>("Commands.Macro", (object)commands);

            return macroCmd;
        }
    }
}
