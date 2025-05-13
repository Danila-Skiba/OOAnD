using App;
using System.Reflection;
namespace SpaceBattle.Lib
{
    public class CreateAdapterCommand : ICommand
    {
        private Type _adapterType;
        public CreateAdapterCommand(Type adapterType)
        {
            _adapterType = adapterType;
        }
        public void Execute()
        {
            var adaptersDict = Ioc.Resolve<IDictionary<string, Assembly>>("Game.Adapter.DictAssembly");
            var key = _adapterType.ToString();

            if (!adaptersDict.TryGetValue(key, out Assembly? assembly))
            {
                var adapterCodeString = Ioc.Resolve<string>("Game.Adapters.Generate", _adapterType);

                var adapterAssembly = Ioc.Resolve<Assembly>("Game.Compile", adapterCodeString);

                adaptersDict[key] = adapterAssembly;
            }
        }
    }
}