namespace CodeExecution.Models;

public sealed record TestItem(
    string Name,
    TestItemStatus Status,
    string? ErrorMessage = null,
    string? Output = null
);

public enum TestItemStatus
{
    Passed,
    Failed
}