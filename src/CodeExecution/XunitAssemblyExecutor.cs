using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Xunit.Runners;

namespace CodeExecution;

public sealed class XunitAssemblyExecutor
{
    public IReadOnlyCollection<TestInfo> Execute(Compilation compilation)
    {
        var outputPath = Path.Combine(Path.GetTempPath(), compilation.SourceModule.Name);

        compilation.Emit(outputPath);
        Assembly.LoadFrom(outputPath);

        var tests = new ConcurrentBag<TestInfo>();
        using var finished = new ManualResetEventSlim();

        var runner = AssemblyRunner.WithoutAppDomain(outputPath);

        runner.OnTestFailed += tests.Add;
        runner.OnTestPassed += tests.Add;

        runner.OnExecutionComplete += _ => finished.Set();

        runner.Start(new AssemblyRunnerStartOptions());
        finished.Wait();

        return tests.ToArray();
    }
}