namespace SpaceBattle.Lib;

public class Rotate : ICommand
{
    private readonly IRotating rotating;

    public Rotate(IRotating rotating)
    {
        this.rotating = rotating;
    }
    public void Execute()
    {
        rotating.CurrentAngle += rotating.AngleVelocity;
    }
}