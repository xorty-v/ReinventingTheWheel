using CodeExecution.Models;
using Xunit.Runners;

namespace CodeExecution;

public sealed class TestResultMapper
{
    public TestResult Map(IReadOnlyCollection<TestInfo> tests)
    {
        ArgumentNullException.ThrowIfNull(tests);

        var items = tests
            .OrderBy(test => test.TypeName)
            .ThenBy(test => test.MethodName)
            .Select(MapItem)
            .ToArray();

        var status = items.All(item => item.Status == TestItemStatus.Passed)
            ? TestResultStatus.Passed
            : TestResultStatus.Failed;

        return new TestResult(
            Status: status,
            Items: items);
    }

    private static TestItem MapItem(TestInfo test)
    {
        return test switch
        {
            TestPassedInfo passed => MapPassedTest(passed),
            TestFailedInfo failed => MapFailedTest(failed),
            _ => throw new ArgumentOutOfRangeException(nameof(test), test.GetType().Name,
                "Unsupported xUnit test result.")
        };
    }

    private static TestItem MapPassedTest(TestPassedInfo test)
    {
        return new TestItem(
            Name: GetTestName(test),
            Status: TestItemStatus.Passed,
            Output: NormalizeOutput(test.Output));
    }

    private static TestItem MapFailedTest(TestFailedInfo test)
    {
        return new TestItem(
            Name: GetTestName(test),
            Status: TestItemStatus.Failed,
            ErrorMessage: NormalizeError(test.ExceptionMessage),
            Output: NormalizeOutput(test.Output));
    }

    private static string GetTestName(TestInfo test)
    {
        if (!string.IsNullOrWhiteSpace(test.MethodName))
            return test.MethodName;

        if (!string.IsNullOrWhiteSpace(test.TestDisplayName))
            return test.TestDisplayName;

        return "Unknown test";
    }

    private static string? NormalizeError(string? value)
    {
        return NormalizeText(value);
    }

    private static string? NormalizeOutput(string? value)
    {
        return NormalizeText(value);
    }

    private static string? NormalizeText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Trim();
    }
}