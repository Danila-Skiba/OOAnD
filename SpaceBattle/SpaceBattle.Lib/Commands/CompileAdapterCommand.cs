using System.Reflection;
using App;
namespace SpaceBattle.Lib
{
    public class CompileAdapterCommand : ICommand
    {
        private readonly Type _adapterType;
        public CompileAdapterCommand(Type adapterType)
        {
            _adapterType = adapterType;
        }
        public void Execute()
        {
            var adaptersDict = Ioc.Resolve<IDictionary<string, Assembly>>("Game.Adapter.DictAssembly");
            var key = _adapterType.Name;
            if (!adaptersDict.TryGetValue(key, out _))
            {
                var adapterCodeString = Ioc.Resolve<string>("Game.Adapters.Generate", _adapterType);

                var adapterAssembly = Ioc.Resolve<Assembly>("Game.Compile", adapterCodeString);

                adaptersDict[key] = adapterAssembly;
            }
        }
    }
}
