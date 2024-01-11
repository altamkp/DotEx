namespace DotnetEx.Maths;

/// <summary>
/// Extensions for <see cref="float"/>.
/// </summary>
public static class FloatExtensions {
    /// <summary>
    /// Trims value to a given precision.
    /// </summary>
    /// <param name="value">Value to trim.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>Trimmed value.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="precision"/> is not positive and smaller than 1.</exception>
    public static float Trim(this float value, float precision = (float)MathDef.DEFAULT_PRECISION) {
        if (precision <= 0 || precision >= 1) {
            throw new ArgumentOutOfRangeException(nameof(precision), "Precision should be positive and smaller than 1.");
        }
        return (float)Math.Round(value / precision) * precision;
    }

    /// <summary>
    /// Checks if the two values are approximately equal.
    /// </summary>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>True if the two values are approximately equal.</returns>
    public static bool IsEqualApprox(this float a, float b, float precision = (float)MathDef.DEFAULT_PRECISION) {
        return a == b || Math.Abs(a - b) < precision;
    }

    /// <summary>
    /// Checks if the value is approximately zero.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="precision">Precision to use.</param>
    /// <returns>True if the value is approximately zero.</returns>
    public static bool IsZeroApprox(this float value, float precision = (float)MathDef.DEFAULT_PRECISION) {
        return IsEqualApprox(value, 0, precision);
    }
}
