namespace CodeExecution.Models;

public sealed record TestResult(
    TestResultStatus Status,
    IReadOnlyList<TestItem> Items,
    string? ErrorMessage = null
);

public enum TestResultStatus
{
    Passed,
    Failed,
    Error
}