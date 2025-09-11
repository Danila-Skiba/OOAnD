using System.Reflection;
using App;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
namespace SpaceBattle.Lib
{

    public class RegisterCompileCommand : ICommand
    {
        public void Execute()
        {
            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Compile", (object[] args) =>
            {
                var code = (string)args[0];
                var syntaxTree = CSharpSyntaxTree.ParseText(code);

                var references = Ioc.Resolve<List<MetadataReference>>("Game.Compile.GetReferences");
                var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

                var compilation = CSharpCompilation.Create($"Generated_{Guid.NewGuid().ToString()}")
                    .AddReferences(references)
                    .WithOptions(options)
                    .AddSyntaxTrees(syntaxTree);

                using var ms = new System.IO.MemoryStream();
                var result = compilation.Emit(ms);
                if (!result.Success)
                {
                    var errors = result.Diagnostics
                        .Select(d => d.ToString());
                    throw new Exception("Error compilation " + string.Join(",", errors));
                }

                ms.Seek(0, System.IO.SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());
                return assembly;
            }).Execute();
        }
    }
}
