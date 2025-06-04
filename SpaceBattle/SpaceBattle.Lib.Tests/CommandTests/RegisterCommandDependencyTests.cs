using System.Reflection;
using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterAdapterCommandTests : IDisposable
    {

        public RegisterAdapterCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Execute_RegistersFactory_AndResolveReturnsMoveCommandWithInjectedAdapter()
        {
            var mockMoving = new Mock<IMoving>();

            Ioc.Resolve<App.ICommand>(
                "IoC.Register", "Game.Adapter.Create",
                (object[] args) => mockMoving.Object).Execute();

            new RegisterAdapterCommand<IMoving, MoveCommand>("Команды.Move").Execute();

            var obj = new Dictionary<string, object>();

            var cmdObj = Ioc.Resolve<object>("Команды.Move", obj);

            Assert.IsType<MoveCommand>(cmdObj);

            var cmd = (MoveCommand)cmdObj;
            var field = typeof(MoveCommand)
                .GetField("moving", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(field);

            var inner = field.GetValue(cmd);
            Assert.Same(mockMoving.Object, inner);
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
