using Microsoft.CodeAnalysis;

namespace CodeExecution;

public sealed class CompilationErrorFormatter
{
    public string Format(IEnumerable<Diagnostic> diagnostics)
    {
        var errors = diagnostics
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .Select(FormatDiagnostic)
            .ToArray();

        return errors.Length == 0
            ? "Compilation failed."
            : string.Join(Environment.NewLine, errors);
    }

    private static string FormatDiagnostic(Diagnostic diagnostic)
    {
        var location = diagnostic.Location;

        if (location == Location.None || location.SourceTree is null)
            return diagnostic.ToString();

        var lineSpan = location.GetLineSpan();
        var fileName = Path.GetFileName(lineSpan.Path);
        var line = lineSpan.StartLinePosition.Line + 1;
        var column = lineSpan.StartLinePosition.Character + 1;

        return $"{fileName}({line},{column}): {diagnostic.Id}: {diagnostic.GetMessage()}";
    }
}