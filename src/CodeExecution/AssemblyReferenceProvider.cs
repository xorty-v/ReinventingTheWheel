using Microsoft.CodeAnalysis;

namespace CodeExecution;

public sealed class AssemblyReferenceProvider
{
    public IReadOnlyCollection<MetadataReference> GetReferences()
    {
        var references = new List<MetadataReference>();

        var trustedAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;

        if (!string.IsNullOrWhiteSpace(trustedAssemblies))
        {
            references.AddRange(
                trustedAssemblies
                    .Split(Path.PathSeparator)
                    .Where(File.Exists)
                    .Select(path => MetadataReference.CreateFromFile(path)));
        }

        AddReferenceIfMissing(references, typeof(object).Assembly.Location);
        AddReferenceIfMissing(references, typeof(Console).Assembly.Location);
        AddReferenceIfMissing(references, typeof(Enumerable).Assembly.Location);

        AddReferenceIfMissing(references, typeof(Xunit.FactAttribute).Assembly.Location);
        AddReferenceIfMissing(references, typeof(Xunit.Assert).Assembly.Location);

        return references;
    }

    private static void AddReferenceIfMissing(List<MetadataReference> references, string? assemblyPath)
    {
        if (string.IsNullOrWhiteSpace(assemblyPath))
            return;

        if (!File.Exists(assemblyPath))
            return;

        var alreadyAdded = references
            .OfType<PortableExecutableReference>()
            .Any(reference => string.Equals(reference.FilePath, assemblyPath, StringComparison.OrdinalIgnoreCase));

        if (!alreadyAdded)
            references.Add(MetadataReference.CreateFromFile(assemblyPath));
    }
}