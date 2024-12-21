namespace SpaceBattle.Lib.Tests;

public class TestsAngle
{
    [Fact]
    public void AngleAddition()
    {
        var angle1 = new Angle(5);
        var angle2 = new Angle(7);
        var result_add = angle1 + angle2;
        var expected = new Angle(4);

        Assert.Equal(expected, result_add);
    }
    [Fact]
    public void AngleEquals()
    {
        var angle1 = new Angle(15);

        var angle2 = new Angle(23);

        Assert.True(angle1.Equals(angle2));
    }
    [Fact]
    public void AngleEqualityOperator()
    {

        var angle1 = new Angle(15);
        var angle2 = new Angle(23);

        Assert.True(angle1 == angle2);
    }
    [Fact]
    public void AngleNotEquals()
    {
        var angle1 = new Angle(1);
        var angle2 = new Angle(2);
        Assert.False(angle1.Equals(angle2));
    }
    [Fact]
    public void AngleUnEqualityOperator()
    {
        var angle1 = new Angle(1);
        var angle2 = new Angle(2);
        Assert.True(angle1 != angle2);
    }
    [Fact]
    public void Angle_GetHashCode()
    {
        var angle = new Angle(5);
        var hashcode = angle.GetHashCode();

        Assert.NotNull(hashcode);
    }
    [Fact]
    public void Angle_Equals_Null_ReturnsFalse()
    {
        var angle1 = new Angle(90);
        Assert.False(angle1.Equals(null));
    }
    [Fact]
    public void Sin_Angle()
    {
        var angle = new Angle(0);

        double Sin_Angle = Math.Sin(angle);

        Assert.Equal(0.0, Sin_Angle);
    }
    [Fact]
    public void Cos_Angle()
    {
        var angle = new Angle(0);

        double Cos_Angle = Math.Cos(angle);

        Assert.Equal(1.0, Cos_Angle);
    }
}