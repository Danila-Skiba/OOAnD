namespace SpaceBattle.Lib
{
    public class MoveCommand : ICommand
    {
        private readonly IMoving moving;
        public MoveCommand(IMoving moving)
        {
            this.moving = moving;
        }
        public void Execute()
        {
            moving.Position += moving.Velocity;
        }
    }
}
