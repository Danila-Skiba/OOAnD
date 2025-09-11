using System.Reflection;
using App;
using App.Scopes;
using Microsoft.CodeAnalysis;
namespace SpaceBattle.Lib.Tests;

public class AdapterTests : IDisposable
{
    public AdapterTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Adapters.Generate", (object[] args) =>
        {
            return @"
using System.Collections.Generic;
using SpaceBattle.Lib;

public class IMovingAdapter : IMoving
{
    private readonly IDictionary<string, object> _dict;

    public IMovingAdapter(IDictionary<string, object> dict)
    {
        _dict = dict;
    }

    public Vector Position
    {
        get { return (Vector)_dict[""Position""]; }
        set { _dict[""Position""] = value; }
    }

    public Vector Velocity
    {
        get { return (Vector)_dict[""Velocity""]; }
    }
}
";
        }).Execute();
    }

    [Fact]
    public void RegisterCompileCommandTest()
    {
        //Arrange
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Compile.GetReferences", (object[] args) => references).Execute();

        var registerCompileCommand = new RegisterCompileCommand();
        registerCompileCommand.Execute();

        //Act & Assert
        var sampleCode = "public class TestClass { public int X { get; set; } }";
        var assembly = Ioc.Resolve<Assembly>("Game.Compile", sampleCode);
        Assert.NotNull(assembly);
        var type = assembly.GetType("TestClass");
        Assert.NotNull(type);
    }

    [Fact]
    public void CompileAdapterCommand_Test()
    {
        //Arrange
        var dictAssembly = new Dictionary<string, Assembly>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Adapter.DictAssembly", (object[] args) => dictAssembly).Execute();
        var registerCompileCommand = new RegisterCompileCommand();
        registerCompileCommand.Execute();

        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IMoving).Assembly.Location),
        };
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Compile.GetReferences", (object[] args) => references).Execute();

        //Act
        var compileAdapterCmd = new CompileAdapterCommand(typeof(IMoving));
        compileAdapterCmd.Execute();

        //Assert
        Assert.True(dictAssembly.ContainsKey("IMoving"));
        var assembly = dictAssembly["IMoving"];
        Assert.NotNull(assembly);
        var adapterType = assembly.GetType("IMovingAdapter");
        Assert.NotNull(adapterType);
    }

    [Fact]
    public void ErrorCompilation_Test()
    {
        //Arrange
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Compile.GetReferences", (object[] args) => references).Execute();

        var registerCompileCommand = new RegisterCompileCommand();
        registerCompileCommand.Execute();

        //Act & Assert
        var invalidCode = "public class TestClass { public int X { get; set; }";
        var exception = Assert.Throws<Exception>(() => Ioc.Resolve<Assembly>("Game.Compile", invalidCode));
        Assert.Contains("Error compilation", exception.Message);
    }

    [Fact]
    public void CompileAdapterCommand_SkipTests()
    {
        //Arrange
        var dictAssembly = new Dictionary<string, Assembly>();
        var mockAssembly = Assembly.GetExecutingAssembly();
        dictAssembly["IMoving"] = mockAssembly;
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Adapter.DictAssembly", (object[] args) => dictAssembly).Execute();

        //Act
        var compileAdapterCmd = new CompileAdapterCommand(typeof(IMoving));
        compileAdapterCmd.Execute();

        //Assert
        Assert.True(dictAssembly.ContainsKey("IMoving"));
        Assert.Equal(mockAssembly, dictAssembly["IMoving"]);
    }

    [Fact]

    public void RegisterCreateAdapter_Test()
    {
        //Arrange
        var dictAssembly = new Dictionary<string, Assembly>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Adapter.DictAssembly", (object[] args) => dictAssembly).Execute();
        var registerCompileCommand = new RegisterCompileCommand();
        registerCompileCommand.Execute();

        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IMoving).Assembly.Location),
        };
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Compile.GetReferences", (object[] args) => references).Execute();

        //Act
        var registerCreateAdapter = new RegisterCreateAdapter();
        registerCreateAdapter.Execute();

        //Assert 1
        var gameObject = new Dictionary<string, object>();
        gameObject["Position"] = new Vector(new int[] { 1, 1 });
        gameObject["Velocity"] = new Vector(new int[] { 1, 1 });
        var adapter = Ioc.Resolve<object>("Game.Adapter.Create", gameObject, typeof(IMoving));
        Assert.NotNull(adapter);
        Assert.IsAssignableFrom<IMoving>(adapter);

        //Assert 2
        var movingAdapter = (IMoving)adapter;
        var newPosition = new Vector(new int[] { 2, 2 });
        movingAdapter.Position = newPosition;
        Assert.Equal(newPosition, gameObject["Position"]);
    }

    public void Dispose()
    {
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Clear").Execute();
    }
}
