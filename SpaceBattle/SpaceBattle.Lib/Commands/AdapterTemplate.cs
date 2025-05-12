namespace SpaceBattle.Lib;

public static class AdapterTemplate
{
    public const string TemplateString = @"
public class {{type}}Adapter : {{type}}
{
    private readonly IDictionary<string, object> _dict;

    public {{type}}Adapter(IDictionary<string, object> dict)
    {
        _dict = dict;
    }

    {{~ for property in properties ~}}
    public {{property.property_type.full_name}} {{property.name}}
    {
        {{~ if property.can_read ~}}
        get
        {
            return Ioc.Resolve<{{property.property_type.full_name}}>(""Object.GetProperty"", _dict, ""{{property.name}}"", typeof({{property.property_type.full_name}}));
        }
        {{~ end ~}}
        {{~ if property.can_write ~}}
        set
        {
            Ioc.Resolve<ICommand>(""Object.SetProperty"", _dict, ""{{property.name}}"", value).Execute();
        }
        {{~ end ~}}
    }
    {{~ end ~}}
}";
}
