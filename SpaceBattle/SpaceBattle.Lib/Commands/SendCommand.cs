namespace SpaceBattle.Lib;

public class SendCommand(ICommand cmd, ICommandReceiver receiver) : ICommand
{
    public void Execute()
    {
        receiver.Receive(cmd);
    }
}

