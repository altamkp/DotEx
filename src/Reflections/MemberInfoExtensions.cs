using System.Reflection;

namespace DotnetEx.Reflections;

/// <summary>
/// Extensions for <see cref="MemberInfo"/>.
/// </summary>
public static class MemberInfoExtensions {
    /// <summary>
    /// Indicates whether custom attributes of a specified type are applied to a specified member.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute to search for.</typeparam>
    /// <param name="memberInfo">Member to inspect.</param>
    /// <returns>True if an attribute of the specified type is applied to element,
    /// otherwise false.</returns>
    public static bool IsDefined<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute {
        return memberInfo.IsDefined(typeof(TAttribute));
    }

    /// <summary>
    /// Indicates whether custom attributes of a specified type are applied to a specified
    /// member, and optionally its ancestors.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute to search for.</typeparam>
    /// <param name="memberInfo">Member to inspect.</param>
    /// <param name="inherit">True to inspect the ancestors of element; otherwise, false.</param>
    /// <returns>True if an attribute of the specified type is applied to element,
    /// otherwise false.</returns>
    public static bool IsDefined<TAttribute>(this MemberInfo memberInfo, bool inherit) where TAttribute : Attribute {
        return memberInfo.IsDefined(typeof(TAttribute), inherit);
    }

    /// <summary>
    /// Gets the type of this member.
    /// </summary>
    /// <param name="memberInfo">Member info object.</param>
    /// <returns>Type of this member.</returns>
    /// <exception cref="ArgumentException">Member other that property, field, or method is provided.</exception>
    public static Type GetMemberType(this MemberInfo memberInfo) {
        Type type;
        if (memberInfo is PropertyInfo propertyInfo && propertyInfo.CanRead) {
            type = propertyInfo.PropertyType;
        } else if (memberInfo is FieldInfo fieldInfo) {
            type = fieldInfo.FieldType;
        } else if (memberInfo is MethodInfo methodInfo) {
            type = methodInfo.ReturnType;
        } else {
            throw new ArgumentException($"Expected property, field or method, not {memberInfo.MemberType}.");
        }
        return type;
    }

    /// <summary>
    /// Returns the property/field value of a specified object.
    /// </summary>
    /// <param name="memberInfo">Member info object</param>
    /// <param name="target">Target object to inspect.</param>
    /// <returns>Property of field value.</returns>
    /// <exception cref="ArgumentException">Member other that property or field is provided.</exception>
    public static object? GetValue(this MemberInfo memberInfo, object target) {
        object? value;
        if (memberInfo is PropertyInfo propertyInfo) {
            if (!propertyInfo.CanRead) {
                throw new InvalidOperationException($"Property {propertyInfo.Name} is write only.");
            }
            value = propertyInfo.GetValue(target);
        } else if (memberInfo is FieldInfo fieldInfo) {
            value = fieldInfo.GetValue(target);
        } else {
            throw new ArgumentException($"Expected property or field, not {memberInfo.MemberType}.");
        }
        return value;
    }

    /// <summary>
    /// Sets the property or field <paramref name="value"/> of a specified <paramref name="target"/> object,
    /// accepts <see cref="PropertyInfo"/> or <see cref="FieldInfo"/> only.
    /// </summary>
    /// <param name="memberInfo">Member info object.</param>
    /// <param name="target">Target object to set.</param>
    /// <param name="value">Value to set.</param>
    /// <exception cref="ArgumentException">Member other that property or field is provided.</exception>
    public static void SetValue(this MemberInfo memberInfo, object target, object value) {
        if (memberInfo is PropertyInfo propertyInfo) {
            if (!propertyInfo.CanWrite) {
                throw new InvalidOperationException($"Property {propertyInfo.Name} is read only.");
            }
            propertyInfo.SetValue(target, value, null);
        } else if (memberInfo is FieldInfo fieldInfo) {
            if (fieldInfo.IsInitOnly) {
                throw new InvalidOperationException($"Field {fieldInfo.Name} is read only.");
            }
            fieldInfo.SetValue(target, value);
        } else {
            throw new ArgumentException($"Expected property or field, not {memberInfo.MemberType}.");
        }
    }
}
