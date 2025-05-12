using App;
using Scriban;

namespace SpaceBattle.Lib
{
    public class AdaptersGenerator : ICommand
    {
        public void Execute()
        {
            var template = Template.Parse(AdapterTemplate.TemplateString);

            Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.Adapters.Generate",
                (object[] args) =>
                {
                    var interfaceType = (Type)args[0];
                    var interfaceProps = interfaceType.GetProperties();
                    var adapterString = template.Render(new
                    {
                        type = interfaceType.Name,
                        properties = interfaceProps.Select(prop => new
                        {
                            name = prop.Name,
                            property_type = new { name = prop.PropertyType.Name, full_name = prop.PropertyType.FullName },
                            can_read = prop.CanRead,
                            can_write = prop.CanWrite
                        }).ToList()
                    });
                    return adapterString;
                }).Execute();
        }
    }
}
