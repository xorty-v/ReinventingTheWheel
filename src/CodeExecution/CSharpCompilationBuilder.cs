using CodeExecution.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeExecution;

public sealed class CSharpCompilationBuilder
{
    private readonly AssemblyReferenceProvider _referenceProvider;

    public CSharpCompilationBuilder(AssemblyReferenceProvider referenceProvider)
    {
        _referenceProvider = referenceProvider;
    }

    public CSharpCompilation Build(CodeTestRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var solutionTree = CreateSyntaxTree(request.SolutionCode, "Solution.cs");
        var testTree = CreateSyntaxTree(request.TestCode, "Tests.cs");

        return CSharpCompilation.Create(
            assemblyName: $"CodeExecution.Tests.{Guid.NewGuid():N}",
            syntaxTrees: [solutionTree, testTree],
            references: _referenceProvider.GetReferences(),
            options: new CSharpCompilationOptions(
                outputKind: OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release,
                warningLevel: 4));
    }

    private static SyntaxTree CreateSyntaxTree(string sourceCode, string fileName)
    {
        if (string.IsNullOrWhiteSpace(sourceCode))
            throw new ArgumentException("Source code cannot be empty.", nameof(sourceCode));

        return CSharpSyntaxTree.ParseText(
            sourceCode,
            path: fileName,
            options: new CSharpParseOptions(LanguageVersion.Latest));
    }
}