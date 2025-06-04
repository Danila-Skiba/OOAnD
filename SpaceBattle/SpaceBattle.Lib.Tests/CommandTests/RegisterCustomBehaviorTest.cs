using App;
using App.Scopes;

namespace SpaceBattle
{
    public class RegisterCustomBehaviorTest
    {
        [Fact]
        public void Execute_ShouldRegisterCustomBehaviorDependency()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

            var obj = new Dictionary<string, object>();
            var customBehavior = new Dictionary<string, Func<object>>();

            var registrator = new RegisterCustomBehaviorDependency();

            registrator.Execute();

            var checker = Ioc.Resolve<IDictionary<string, object>>("CustomBehaviorWrapper", obj, customBehavior);

            Assert.IsAssignableFrom<IDictionary<string, object>>(checker);
        }
    }
}
