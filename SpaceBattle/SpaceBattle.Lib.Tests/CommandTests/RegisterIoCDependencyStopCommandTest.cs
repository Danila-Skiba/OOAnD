using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyStopCommandTest: IDisposable
    {
        public RegisterIoCDependencyStopCommandTest()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }
         [Fact]
        public void Execute_ShouldRegisterIoCDependencyStopCommandDependency()
        {

            var registerCommand = new RegisterIoCDependencyActionsStop();

            registerCommand.Execute();
            Assert.IsType<StopCommand>(Ioc.Resolve<ICommand>("Actions.Stop", "objId", "cmdName"));


        
        }       
        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
