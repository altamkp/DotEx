using System.Reflection;

namespace DotEx.Reflections;

/// <summary>
/// Extensions for <see cref="Assembly"/>.
/// </summary>
public static class AssemblyExtensions {
    /// <summary>
    /// Returns all derived types of the provided base type. Return abstract types if
    /// <paramref name="allowAbstract"/> is true.
    /// </summary>
    /// <param name="assembly">Assembly to search.</param>
    /// <param name="baseType">Base type to compare.</param>
    /// <param name="allowAbstract">Allow abstract derived types.</param>
    /// <returns>Derived types of the base type.</returns>
    public static IEnumerable<Type> GetDerivedTypes(this Assembly assembly, Type baseType, bool allowAbstract) {
        return assembly.GetTypes().Where(type => 
            baseType.IsAssignableFrom(type) && (allowAbstract || !type.IsAbstract));
    }

    /// <summary>
    /// Returns all derived types of the provided base type. Return abstract types if
    /// <paramref name="allowAbstract"/> is true.
    /// </summary>
    /// <typeparam name="T">Base type to compare.</typeparam>
    /// <param name="assembly">Assembly to search.</param>
    /// <param name="allowAbstract">Allow abstract derived types.</param>
    /// <returns>Derived types of the base type.</returns>
    public static IEnumerable<Type> GetDerivedTypes<T>(this Assembly assembly, bool allowAbstract) {
        var baseType = typeof(T);
        return assembly.GetTypes().Where(type => 
            baseType.IsAssignableFrom(type) && (allowAbstract || !type.IsAbstract));
    }

    /// <summary>
    /// Returns all types in the assembly annotated with the given attribute. Return abstract 
    /// types if <paramref name="allowAbstract"/> is true.
    /// </summary>
    /// <param name="assembly">Assembly to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="allowAbstract">Allow abstract types, false by default</param>
    /// <returns>Types with the given attribute defined.</returns>
    public static IEnumerable<Type> GetTypesWithAttribute(this Assembly assembly, Type attrType, bool allowAbstract = false) {
        return assembly.GetTypes().Where(type =>
            type.IsDefined(attrType, true) && (allowAbstract || !type.IsAbstract));
    }

    /// <summary>
    /// Returns all types in the assembly annotated with the given attribute. Return abstract 
    /// types if <paramref name="allowAbstract"/> is true.
    /// </summary>
    /// <typeparam name="T">Attribute to match.</typeparam>
    /// <param name="assembly">Assembly to search.</param>
    /// <param name="allowAbstract">Allow abstract types, false by default</param>
    /// <returns>Types with the given attribute defined.</returns>
    public static IEnumerable<Type> GetTypesWithAttribute<T>(this Assembly assembly, bool allowAbstract = false) where T : Attribute {
        return assembly.GetTypes().Where(type =>
            type.IsDefined(typeof(T), true) && (allowAbstract || !type.IsAbstract));
    }

    /// <summary>
    /// Returns all types and their respective attributes in the assembly annotated with the given attribute. 
    /// Return abstract types if <paramref name="allowAbstract"/> is true.
    /// </summary>
    /// <param name="assembly">Assembly to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="allowAbstract">Allow abstract types, false by default</param>
    /// <returns>Types with the given attribute defined, together with their respective attributes.</returns>
    public static IEnumerable<(Type type, Attribute[] attributes)> GetTypesAndAttributes(this Assembly assembly, Type attrType, bool allowAbstract = false) {
        foreach (var type in assembly.GetTypes()) {
            var attributes = type.GetCustomAttributes(attrType, true)
                .Select(attribute => (Attribute)attribute);
            if (attributes.Any() && (allowAbstract || !type.IsAbstract)) {
                yield return (type, attributes.ToArray());
            }
        }
    }

    /// <summary>
    /// Returns all types and their respective attributes in the assembly annotated with the given attribute. 
    /// Return abstract types if <paramref name="allowAbstract"/> is true.
    /// </summary>
    /// <typeparam name="T">Attribute to match.</typeparam>
    /// <param name="assembly">Assembly to search.</param>
    /// <param name="allowAbstract">Allow abstract types, false by default</param>
    /// <returns>Types with the given attribute defined.</returns>
    public static IEnumerable<(Type type, T[] attributes)> GetTypesAndAttributes<T>(this Assembly assembly, bool allowAbstract = false) where T : Attribute {
        var attrType = typeof(T);
        foreach (Type type in assembly.GetTypes()) {
            var attributes = type.GetCustomAttributes(attrType, true)
                .Select(attribute => (T)attribute);
            if (attributes.Any() && (allowAbstract || !type.IsAbstract)) {
                yield return (type, attributes.ToArray());
            }
        }
    }
}
