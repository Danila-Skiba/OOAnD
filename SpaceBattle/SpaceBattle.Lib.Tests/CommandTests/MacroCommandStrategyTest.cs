using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class MacroCommandStrategyTests : IDisposable
    {
        public MacroCommandStrategyTests()
        {

            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void MacroCommandStrategyTestTrue()
        {

            Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Macro.Test", (object[] args) => new[] { "Commands.Rotate", "Commands.Move" }).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Rotate", (object[] args) => new Rotate((IRotating)args[0])).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Move", (object[] args) => new MoveCommand((IMoving)args[0])).Execute();

            var strategy = new CreateMacroCommandStrategy("Specs.Macro.Test");

            var rotating = new Mock<IRotating>();
            rotating.Setup(r => r.CurrentAngle).Returns(new Angle(45));
            rotating.Setup(r => r.AngleVelocity).Returns(new Angle(90));

            var moving = new Mock<IMoving>();
            moving.Setup(m => m.Position).Returns(new Vector(new int[] { 1, 1 }));
            moving.Setup(m => m.Velocity).Returns(new Vector(new int[] { 1, 1 }));

            var macroCommand = strategy.Resolve(new object[] { rotating.Object, moving.Object });
            macroCommand.Execute();

            moving.VerifySet(r => r.Position = new Vector(new int[] { 2, 2 }));
            rotating.VerifySet(a => a.CurrentAngle = new Angle(135));
        }

        [Fact]
        public void Resolve_ThrowsException_WhenCommandNotRegistered()
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Macro.Test", (object[] args) => new[] { "Commands.Rotate", "Commands.Unknown" }).Execute();

            var strategy = new CreateMacroCommandStrategy("Specs.Macro.Test");
            var rotating = new Mock<IRotating>();
            var moving = new Mock<IMoving>();

            Assert.Throws<Exception>(() => strategy.Resolve(new object[] { rotating.Object, moving.Object }));
        }

        [Fact]
        public void Resolve_ThrowsException_WhenInvalidArgumentsProvided()
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Macro.Test", (object[] args) => new[] { "Commands.Rotate" }).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Rotate", (object[] args) => new Rotate((IRotating)args[0])).Execute();

            var strategy = new CreateMacroCommandStrategy("Specs.Macro.Test");
            var exception = Assert.Throws<InvalidOperationException>(() => strategy.Resolve(new object[] { null }));

            Assert.Equal("Команда 'Commands.Rotate' принимает аргумент null.", exception.Message);
        }
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
