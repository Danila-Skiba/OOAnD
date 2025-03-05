namespace SpaceBattle.Lib
{
    public class Game : ICommand, ICommandReceiver
    {
        private readonly Queue<ICommand> _commands = new Queue<ICommand>();

        public void Receive(ICommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd), "Команда не может быть null.");
            }

            _commands.Enqueue(cmd);
        }

        public void Execute()
        {
            while (_commands.Count > 0)
            {
                var cmd = _commands.Dequeue();
                if (cmd == null)
                {
                    throw new InvalidOperationException("Обнаружена null-команда в очереди.");
                }

                try
                {
                    cmd.Execute();
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Ошибка при выполнении команды: {ex.Message}");
                }
            }
        }
    }
}
