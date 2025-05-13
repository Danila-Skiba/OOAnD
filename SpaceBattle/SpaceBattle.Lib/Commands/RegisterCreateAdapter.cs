using System.Reflection;
using App;
namespace SpaceBattle.Lib
{
    public class RegisterCreateAdapter : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Adapter.Create", (object[] args) =>
            {
                var gameObject = (IDictionary<string, object>)args[0];
                var type = (Type)args[1];
                var compileAdapterCmd = new CompileAdapterCommand(type);
                compileAdapterCmd.Execute();

                var typeName = $"{type.Name}Adapter";

                var dictAssembly = Ioc.Resolve<IDictionary<string, Assembly>>("Game.Adapter.DictAssembly");

                var adapterAssembly = dictAssembly[type.Name];
                return Activator.CreateInstance(adapterAssembly.GetType(typeName)!, gameObject);
            }).Execute();
        }
    }
}
