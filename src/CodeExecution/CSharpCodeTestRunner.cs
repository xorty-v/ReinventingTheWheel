using CodeExecution.Models;
using Microsoft.CodeAnalysis;

namespace CodeExecution;

public sealed class CSharpCodeTestRunner
{
    private readonly CSharpCompilationBuilder _compilationBuilder;
    private readonly XunitAssemblyExecutor _assemblyExecutor;
    private readonly TestResultMapper _resultMapper;
    private readonly CompilationErrorFormatter _errorFormatter;

    public CSharpCodeTestRunner()
        : this(
            new CSharpCompilationBuilder(new AssemblyReferenceProvider()),
            new XunitAssemblyExecutor(),
            new TestResultMapper(),
            new CompilationErrorFormatter())
    {
    }

    public CSharpCodeTestRunner(
        CSharpCompilationBuilder compilationBuilder,
        XunitAssemblyExecutor assemblyExecutor,
        TestResultMapper resultMapper,
        CompilationErrorFormatter errorFormatter)
    {
        _compilationBuilder = compilationBuilder;
        _assemblyExecutor = assemblyExecutor;
        _resultMapper = resultMapper;
        _errorFormatter = errorFormatter;
    }

    public TestResult Run(CodeTestRequest request)
    {
        var compilation = _compilationBuilder.Build(request);

        var errors = compilation.GetDiagnostics()
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();

        if (errors.Length > 0)
        {
            return new TestResult(
                Status: TestResultStatus.Error,
                Items: [],
                ErrorMessage: _errorFormatter.Format(errors));
        }

        var tests = _assemblyExecutor.Execute(compilation);

        return _resultMapper.Map(tests);
    }
}