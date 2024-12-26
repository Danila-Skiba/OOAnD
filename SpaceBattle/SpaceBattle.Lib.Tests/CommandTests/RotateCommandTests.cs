using Moq;
namespace SpaceBattle.Lib.Tests;

public class TestsRotateCommand
{
    [Fact]
    public static void RotateTest()
    {
        var rotating = new Mock<IRotating>();

        rotating.SetupGet(r => r.CurrentAngle).Returns(new Angle(45));
        rotating.SetupGet(r => r.AngleVelocity).Returns(new Angle(90));

        var rotate_cmd = new Rotate(rotating.Object);

        rotate_cmd.Execute();

        rotating.VerifySet(r => r.CurrentAngle = new Angle(135));
    }

    [Fact]
    public void Rotate_CantReadPosition()
    {
        var rotating = new Mock<IRotating>();

        rotating.SetupGet(r => r.CurrentAngle).Throws<InvalidOperationException>();
        rotating.SetupGet(r => r.AngleVelocity).Returns(new Angle(90));

        Assert.Throws<InvalidOperationException>(() => new Rotate(rotating.Object).Execute());
    }

    [Fact]
    public void Rotate_CantReadVelocity()
    {
        var rotating = new Mock<IRotating>();

        rotating.SetupGet(r => r.CurrentAngle).Returns(new Angle(45));
        rotating.SetupGet(r => r.AngleVelocity).Throws<InvalidOperationException>();

        Assert.Throws<InvalidOperationException>(() => new Rotate(rotating.Object).Execute());
    }

    [Fact]
    public void Rotate_CantWritePosition()
    {

        var rotating = new Mock<IRotating>();

        rotating.SetupGet(r => r.CurrentAngle).Returns(new Angle(45));
        rotating.SetupGet(r => r.AngleVelocity).Returns(new Angle(90));
        rotating.SetupSet(r => r.CurrentAngle = new Angle(135)).Throws<InvalidOperationException>();

        Assert.Throws<InvalidOperationException>(() => new Rotate(rotating.Object).Execute());
    }
}
