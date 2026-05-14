namespace LINQ;

public static class LinqExtensions
{
    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (predicate(item))
                yield return item;
        }
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(
        this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        foreach (TSource item in source)
            yield return selector(item);
    }

    public static int Count<TSource>(this IEnumerable<TSource> source)
    {
        int count = 0;

        foreach (TSource _ in source)
            count++;

        return count;
    }

    public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        int count = 0;

        foreach (TSource item in source)
        {
            if (predicate(item))
                count++;
        }

        return count;
    }

    public static bool Any<TSource>(this IEnumerable<TSource> source)
    {
        using IEnumerator<TSource> enumerator = source.GetEnumerator();

        return enumerator.MoveNext();
    }

    public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (predicate(item))
                return true;
        }

        return false;
    }

    public static TSource First<TSource>(this IEnumerable<TSource> source)
    {
        foreach (TSource item in source)
            return item;

        throw new InvalidOperationException("Sequence contains no elements.");
    }

    public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (predicate(item))
                return item;
        }

        throw new InvalidOperationException("Sequence contains no matching element.");
    }

    public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source)
    {
        foreach (TSource item in source)
            return item;

        return default;
    }

    public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (predicate(item))
                return item;
        }

        return default;
    }

    public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        foreach (TSource item in source)
        {
            if (!predicate(item))
                return false;
        }

        return true;
    }

    public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
    {
        int taken = 0;

        foreach (TSource item in source)
        {
            if (taken >= count)
                yield break;

            yield return item;
            taken++;
        }
    }

    public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
    {
        int skipped = 0;

        foreach (TSource item in source)
        {
            if (skipped < count)
            {
                skipped++;
                continue;
            }

            yield return item;
        }
    }

    public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
    {
        List<TSource> result = new();

        foreach (TSource item in source)
            result.Add(item);

        return result;
    }
}