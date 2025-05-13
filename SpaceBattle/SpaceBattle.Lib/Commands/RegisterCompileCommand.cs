using App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

                var references = Ioc.Resolve<MetadataReference>("Game.Compile.GerReferences");
                var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

                var compilation = CSharpCompilation.Create($"Generated_{Guid.NewGuid().ToString()}").AddReferences(references).WithOptions(options).AddSyntaxTrees(syntaxTree);


                using var ms = new System.IO.MemoryStream();
                var result = compilation.Emit(ms);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                var assembly = Assembly.Load(ms.ToArray());

                return assembly;

            }).Execute();
        }
    }
}