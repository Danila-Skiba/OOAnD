using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyActionsStopTests: IDisposable
    {
        public RegisterIoCDependencyActionsStopTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }
        [Fact]
        public void ShouldRegisterStopCommandDependency()
        {

            new RegisterIoCDependencyActionsStop().Execute();
            Assert.IsType<StopCommand>(Ioc.Resolve<App.ICommand>("Actions.Stop", "testObjId", "testCmdName"));
        }
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
