using App;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class StopCommandTest
    {

        [Fact]
        public void ShouldInjectEmptyCommand()
        {

            var mockInjectable = new Mock<ICommandInjectable>();
            var mockEmptyCommand = new Mock<ICommand>();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Object.GetInjectable", (object[] args) => mockInjectable.Object).Execute();
            Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Empty", (object[] args) => mockEmptyCommand.Object).Execute();
            new StopCommand("testObjId", "testCmdName").Execute();
            mockInjectable.Verify(i => i.Inject(mockEmptyCommand.Object), Times.Once);
        }
    }
}
