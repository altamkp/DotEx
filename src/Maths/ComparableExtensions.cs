namespace DotnetEx.Maths;

/// <summary>
/// Extensions for <see cref="IComparable"/>.
/// </summary>
public static class ComparableExtensions {
    /// <summary>
    /// Checks if <paramref name="value"/> is between the inclusive 
    /// <paramref name="lower"/> and <paramref name="upper"/> limits.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="lower">Lower limit for reference.</param>
    /// <param name="upper">Upper limit for reference.</param>
    /// <returns>True if value is between or equal to the limits.</returns>
    public static bool IsBetween(this IComparable value, IComparable lower, IComparable upper) {
        return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
    }
}
