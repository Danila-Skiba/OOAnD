namespace SpaceBattle.Lib;

public interface IRotating
{
    Angle CurrentAngle { get; set; }
    Angle AngleVelocity { get; }
}
