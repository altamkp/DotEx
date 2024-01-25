using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DotEx.Reflections;

/// <summary>
/// Extensions for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions {
    /// <summary>
    /// Gets the interface type from <paramref name="genericInterfaceType"/>.
    /// </summary>
    /// <param name="type">Type to inspect.</param>
    /// <param name="genericInterfaceType">Generic interface type to match.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"><paramref name="type"/> does not implement
    /// <paramref name="genericInterfaceType"/></exception>
    public static Type GetGenericInterfaceType(this Type type, Type genericInterfaceType) {
        if (!type.TryGetGenericInterfaceType(genericInterfaceType, out var interfaceType)) {
            throw new ArgumentException($"{type.Name} does not implement {genericInterfaceType.Name}.");
        }
        return interfaceType;
    }

    /// <summary>
    /// Checks if type implements <paramref name="genericInterfaceType"/>.
    /// </summary>
    /// <param name="type">Type to inspect.</param>
    /// <param name="genericInterfaceType">Generic interface type to match.</param>
    /// <param name="interfaceType">Interface type if successfully matched, otherwise null.</param>
    /// <returns>True if interface type successfully matched, otherwise false.</returns>
    /// <exception cref="ArgumentException"><paramref name="genericInterfaceType"/>
    /// is not a generic interface type.</exception>
    public static bool TryGetGenericInterfaceType(this Type type, Type genericInterfaceType, [NotNullWhen(true)] out Type? interfaceType) {
        if (!genericInterfaceType.IsInterface || !genericInterfaceType.IsGenericType) {
            throw new ArgumentException($"{genericInterfaceType.Name} is not a generic interface type.");
        }
        interfaceType = type.GetInterfaces().SingleOrDefault(i => 
            i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType);
        return interfaceType != null;
    }

    /// <summary>
    /// Returns all members in the type annotated with the given attribute.
    /// </summary>
    /// <param name="type">Type to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Members with the given attribute defined.</returns>
    public static IEnumerable<MemberInfo> GetMembersWithAttribute(this Type type, Type attrType, BindingFlags? flags = null) {
        var members = flags == null ? type.GetMembers() : type.GetMembers(flags.Value);
        return members.Where(mi => mi.IsDefined(attrType));
    }

    /// <summary>
    /// Returns all members in the type annotated with the given attribute.
    /// </summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="type">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Members with the given attribute defined.</returns>
    public static IEnumerable<MemberInfo> GetMembersWithAttribute<T>(this Type type, BindingFlags? flags = null) where T : Attribute {
        var members = flags == null ? type.GetMembers() : type.GetMembers(flags.Value);
        return members.Where(mi => mi.IsDefined<T>());
    }

    /// <summary>
    /// Returns all members and their respective attributes in the type annotated with the given attribute.
    /// </summary>
    /// <param name="type">Type to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Members with the given attribute defined, together with the respective attributes..</returns>
    public static IEnumerable<(MemberInfo MemberInfo, Attribute[] Attributes)> GetMembersAndAttributes(this Type type, Type attrType, BindingFlags? flags = null) {
        var members = flags == null ? type.GetMembers() : type.GetMembers(flags.Value);
        foreach (var member in members) {
            var attributes = member
                .GetCustomAttributes(attrType, true)
                .Select(attr => (Attribute)attr);
            if (attributes.Any()) {
                yield return (member, attributes.ToArray());
            }
        }
    }

    /// <summary>
    /// Returns all members and their respective attributes in the type annotated with the given attribute.
    /// </summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="type">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Members with the given attribute defined, together with the respective attributes..</returns>
    public static IEnumerable<(MemberInfo MemberInfo, T[] Attributes)> GetMembersAndAttributes<T>(this Type type, BindingFlags? flags = null) where T : Attribute {
        var members = flags == null ? type.GetMembers() : type.GetMembers(flags.Value);
        var attrType = typeof(T);
        foreach (var member in members) {
            var attributes = member
                .GetCustomAttributes(attrType, true)
                .Select(attr => (T)attr);
            if (attributes.Any()) {
                yield return (member, attributes.ToArray());
            }
        }
    }

    /// <summary>
    /// Returns all properties in the type annotated with the given attribute.
    /// </summary>
    /// <param name="type">Type to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Properties with the given attribute defined.</returns>
    public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(this Type type, Type attrType, BindingFlags? flags = null) {
        var properties = flags == null ? type.GetProperties() : type.GetProperties(flags.Value);
        return properties.Where(pi => pi.IsDefined(attrType));
    }

    /// <summary>
    /// Returns all properties in the type annotated with the given attribute.
    /// </summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="type">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Properties with the given attribute defined.</returns>
    public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this Type type, BindingFlags? flags = null) where T : Attribute {
        var properties = flags == null ? type.GetProperties() : type.GetProperties(flags.Value);
        return properties.Where(pi => pi.IsDefined<T>());
    }

    /// <summary>
    /// Returns all properties and their respective attributes in the type annotated with the given attribute.
    /// </summary>
    /// <param name="type">Type to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Properties with the given attribute defined, together with the respective attributes..</returns>
    public static IEnumerable<(PropertyInfo PropertyInfo, Attribute[] Attributes)> GetPropertiesAndAttributes(this Type type, Type attrType, BindingFlags? flags = null) {
        var properties = flags == null ? type.GetProperties() : type.GetProperties(flags.Value);
        foreach (var property in properties) {
            var attributes = property
                .GetCustomAttributes(attrType, true)
                .Select(attr => (Attribute)attr);
            if (attributes.Any()) {
                yield return (property, attributes.ToArray());
            }
        }
    }

    /// <summary>
    /// Returns all properties and their respective attributes in the type annotated with the given attribute.
    /// </summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="type">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Properties with the given attribute defined, together with the respective attributes..</returns>
    public static IEnumerable<(PropertyInfo PropertyInfo, T[] Attributes)> GetPropertiesAndAttributes<T>(this Type type, BindingFlags? flags = null) where T : Attribute {
        var attrType = typeof(T);
        var properties = flags == null ? type.GetProperties() : type.GetProperties(flags.Value);
        foreach (var property in properties) {
            var attributes = property
                .GetCustomAttributes(attrType, true)
                .Select(attr => (T)attr);
            if (attributes.Any()) {
                yield return (property, attributes.ToArray());
            }
        }
    }

    /// <summary>
    /// Returns all fields in the type annotated with the given attribute.
    /// </summary>
    /// <param name="type">Type to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Fields with the given attribute defined.</returns>
    public static IEnumerable<FieldInfo> GetFieldsWithAttribute(this Type type, Type attrType, BindingFlags? flags = null) {
        var fields = flags == null ? type.GetFields() : type.GetFields(flags.Value);
        return fields.Where(pi => pi.IsDefined(attrType));
    }

    /// <summary>
    /// Returns all fields in the type annotated with the given attribute.
    /// </summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="type">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Fields with the given attribute defined.</returns>
    public static IEnumerable<FieldInfo> GetFieldsWithAttribute<T>(this Type type, BindingFlags? flags = null) where T : Attribute {
        var fields = flags == null ? type.GetFields() : type.GetFields(flags.Value);
        return fields.Where(pi => pi.IsDefined<T>());
    }

    /// <summary>
    /// Returns all fields and their respective attributes in the type annotated with the given attribute.
    /// </summary>
    /// <param name="type">Type to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Fields with the given attribute defined, together with the respective attributes..</returns>
    public static IEnumerable<(FieldInfo FieldInfo, Attribute[] Attributes)> GetFieldsAndAttributes(this Type type, Type attrType, BindingFlags? flags = null) {
        var fields = flags == null ? type.GetFields() : type.GetFields(flags.Value);
        foreach (var field in fields) {
            var attributes = field
                .GetCustomAttributes(attrType, true)
                .Select(attr => (Attribute)attr);
            if (attributes.Any()) {
                yield return (field, attributes.ToArray());
            }
        }
    }

    /// <summary>
    /// Returns all fields and their respective attributes in the type annotated with the given attribute.
    /// </summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="type">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Fields with the given attribute defined, together with the respective attributes..</returns>
    public static IEnumerable<(FieldInfo FieldInfo, T[] Attributes)> GetFieldsAndAttributes<T>(this Type type, BindingFlags? flags = null) where T : Attribute {
        var attrType = typeof(T);
        var fields = flags == null ? type.GetFields() : type.GetFields(flags.Value);
        foreach (var field in fields) {
            var attributes = field
                .GetCustomAttributes(attrType, true)
                .Select(attr => (T)attr);
            if (attributes.Any()) {
                yield return (field, attributes.ToArray());
            }
        }
    }

    /// <summary>
    /// Returns all methods in the type annotated with the given attribute.
    /// </summary>
    /// <param name="type">Type to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Methods with the given attribute defined.</returns>
    public static IEnumerable<MethodInfo> GetMethodsWithAttribute(this Type type, Type attrType, BindingFlags? flags = null) {
        var methods = flags == null ? type.GetMethods() : type.GetMethods(flags.Value);
        return type.GetMethods().Where(pi => pi.IsDefined(attrType));
    }

    /// <summary>
    /// Returns all methods in the type annotated with the given attribute.
    /// </summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="type">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Methods with the given attribute defined.</returns>
    public static IEnumerable<MethodInfo> GetMethodsWithAttribute<T>(this Type type, BindingFlags? flags = null) where T : Attribute {
        var methods = flags == null ? type.GetMethods() : type.GetMethods(flags.Value);
        return type.GetMethods().Where(pi => pi.IsDefined<T>());
    }

    /// <summary>
    /// Returns all methods and their respective attributes in the type annotated with the given attribute.
    /// </summary>
    /// <param name="type">Type to search.</param>
    /// <param name="attrType">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Methods with the given attribute defined, together with the respective attributes..</returns>
    public static IEnumerable<(MethodInfo MethodInfo, Attribute[] Attributes)> GetMethodsAndAttributes(this Type type, Type attrType, BindingFlags? flags = null) {
        var methods = flags == null ? type.GetMethods() : type.GetMethods(flags.Value);
        foreach (var method in methods) {
            var attributes = method
                .GetCustomAttributes(attrType, true)
                .Select(attr => (Attribute)attr);
            if (attributes.Any()) {
                yield return (method, attributes.ToArray());
            }
        }
    }

    /// <summary>
    /// Returns all methods and their respective attributes in the type annotated with the given attribute.
    /// </summary>
    /// <typeparam name="T">Type to search.</typeparam>
    /// <param name="type">Attribute to match.</param>
    /// <param name="flags">Binding flags to match.</param>
    /// <returns>Methods with the given attribute defined, together with the respective attributes..</returns>
    public static IEnumerable<(MethodInfo MethodInfo, T[] Attributes)> GetMethodsAndAttributes<T>(this Type type, BindingFlags? flags = null) where T : Attribute {
        var attrType = typeof(T);
        var methods = flags == null ? type.GetMethods() : type.GetMethods(flags.Value);
        foreach (var method in methods) {
            var attributes = method
                .GetCustomAttributes(attrType, true)
                .Select(attr => (T)attr);
            if (attributes.Any()) {
                yield return (method, attributes.ToArray());
            }
        }
    }
}
