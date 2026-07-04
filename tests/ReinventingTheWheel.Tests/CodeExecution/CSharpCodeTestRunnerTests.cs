using CodeExecution;
using CodeExecution.Models;

namespace ReinventingTheWheel.Tests.CodeExecution;

public class CSharpCodeTestRunnerTests
{
    [Fact]
    public void Run_ShouldReturnPassed_WhenSolutionPassesAllTests()
    {
        var solutionCode = """
                           public static class Solution
                           {
                               public static int Add(int left, int right)
                               {
                                   return left + right;
                               }
                           }
                           """;

        var testCode = """
                       using Xunit;

                       public sealed class SolutionTests
                       {
                           [Fact]
                           public void Add_ShouldReturnSum()
                           {
                               Assert.Equal(4, Solution.Add(2, 2));
                           }
                       }
                       """;

        var runner = new CSharpCodeTestRunner();

        var result = runner.Run(new CodeTestRequest(solutionCode, testCode));

        Assert.Equal(TestResultStatus.Passed, result.Status);
        Assert.Single(result.Items);
        Assert.Equal(TestItemStatus.Passed, result.Items[0].Status);
    }
}