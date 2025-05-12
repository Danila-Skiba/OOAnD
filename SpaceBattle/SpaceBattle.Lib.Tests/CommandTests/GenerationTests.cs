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
            return Ioc.Resolve<SpaceBattle.Lib.Vector>(""Object.GetProperty"", _dict, ""Position"", typeof(SpaceBattle.Lib.Vector));
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
            return Ioc.Resolve<SpaceBattle.Lib.Vector>(""Object.GetProperty"", _dict, ""Velocity"", typeof(SpaceBattle.Lib.Vector));
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
