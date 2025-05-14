using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class GenerationTests : IDisposable
    {
        public GenerationTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void GenerateAdapter_ReturnsCorrectString()
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", "Object.GetProperty", (object[] args) =>
            {
                var dict = (IDictionary<string, object>)args[0];
                var property = args[1];
                if (dict.TryGetValue("Position", out var value) && value is Vector vector)
                {
                    return vector;
                }

                if (dict.TryGetValue("Velocity", out var value2) && value2 is Vector vector2)
                {
                    return vector2;
                }

                return new Vector(new int[] { 0, 0 });
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Object.SetProperty", (object[] args) =>
            {
                var dict = (IDictionary<string, object>)args[0];
                var propertyName = (string)args[1];
                var value = args[2];
                return new Action(() =>
                {
                    if (value is Vector vector)
                    {
                        dict[propertyName] = vector;
                    }
                    else
                    {
                        throw new NotSupportedException($"Type {value.GetType().Name} is not supported");
                    }
                });
            }).Execute();

            new AdaptersGenerator().Execute();

            var generatedCode = Ioc.Resolve<string>("Game.Adapters.Generate", typeof(IMoving));

            var expected = @"
public class IMovingAdapter : IMoving
{
    private readonly IDictionary<string, object> _dict;

    public IMovingAdapter(IDictionary<string, object> dict)
    {
        _dict = dict;
    }

    public SpaceBattle.Lib.Vector Position
    {
        get
        {
            return Ioc.Resolve<SpaceBattle.Lib.Vector>(""Object.GetProperty"", _dict, property);
        }
        set
        {
            Ioc.Resolve<ICommand>(""Object.SetProperty"", _dict, ""Position"", value).Execute();
        }
    }
    public SpaceBattle.Lib.Vector Velocity
    {
        get
        {
            return Ioc.Resolve<SpaceBattle.Lib.Vector>(""Object.GetProperty"", _dict, property);
        }
    }
}";

            Assert.Equal(expected.Trim(), generatedCode.Trim());
        }

        public void Dispose()
        {
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
        }
    }
}
