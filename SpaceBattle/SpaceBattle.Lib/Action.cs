namespace SpaceBattle.Lib
{
    public class Action : ICommand
    {
        private readonly System.Action _execute;
        public Action(System.Action execute)
        {
            _execute = execute;
        }

        public void Execute()
        {
            _execute();
        }
    }
}
