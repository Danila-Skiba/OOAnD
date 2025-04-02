using App;

namespace SpaceBattle.Lib
{
    public class GameReceiver : ICommandReceiver
    {
        public void Receive(ICommand? cmd)
        {
            var queue = Ioc.Resolve<Queue<ICommand>>("Game.CommandsQueue");
            queue.Enqueue(cmd);
        }
    }
}
