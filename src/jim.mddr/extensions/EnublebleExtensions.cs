namespace Jim.Mddr.Extensions;

public static class EnumerableExtensions
{

    /// <summary>
    /// Determines whether the collection has any elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool HasElements<T>(this IEnumerable<T> source)
    {
        return source != null && source.Any();
    }

    /// <summary>
    /// Determines whether the collection has no elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool HasNoElements<T>(this IEnumerable<T> source)
    {
        return !source.HasElements();
    }   
}
