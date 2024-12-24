using Moq;
namespace SpaceBattle.Lib.Tests
{
    public class MoveTest
    {
        [Fact]
        public void TestObjectMove()
        {
            var obj_moving = new Mock<IMoving>();

            obj_moving.SetupGet(a => a.Position).Returns(new Vector(new int[] { 12, 5 }));
            obj_moving.SetupGet(a => a.Velocity).Returns(new Vector(new int[] { -4, 1 }));

            var command = new MoveCommand(obj_moving.Object);
            command.Execute();

            obj_moving.VerifySet(a => a.Position = new Vector(new int[] { 8, 6 }));
        }

        [Fact]
        public void TestPositionObjectCannotRead()
        {
            var obj_moving = new Mock<IMoving>();
            obj_moving.SetupGet(a => a.Position).Throws<InvalidOperationException>();

            var command = new MoveCommand(obj_moving.Object);

            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Fact]
        public void TestVelocityObjectCannotRead()
        {
            var obj_moving = new Mock<IMoving>();

            obj_moving.SetupGet(a => a.Position).Returns(new Vector(new int[] { 12, 5 }));
            obj_moving.SetupGet(a => a.Velocity).Throws<InvalidOperationException>();

            var command = new MoveCommand(obj_moving.Object);

            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Fact]
        public void TestImpossibleChangePositionObject()
        {
            var obj_moving = new Mock<IMoving>();

            obj_moving.SetupGet(a => a.Position).Returns(new Vector(new int[] { 12, 5 }));
            obj_moving.SetupGet(a => a.Velocity).Returns(new Vector(new int[] { -7, 3 }));

            obj_moving.SetupSet(a => a.Position = new Vector(new int[] { 5, 8 })).Throws<InvalidOperationException>();

            var command = new MoveCommand(obj_moving.Object);

            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }
    }
}
