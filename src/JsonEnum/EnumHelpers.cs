using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json;

namespace JsonEnum;

/// <summary>
/// Enum extensions
/// </summary>
public static class EnumHelpers
{
    static object? GetAttribute(Type enumType, Type attributeType, string? value) =>
        value is null || !enumType.IsEnum
            ? null
            : enumType
                .GetMember(value)
                .SelectMany(p => p.GetCustomAttributes(attributeType, true))
                .FirstOrDefault();

    static TAttr? GetAttribute<TEnum, TAttr>(this TEnum value)
        where TEnum : Enum
        where TAttr : Attribute =>
        GetAttribute(value.GetType(), typeof(TAttr), value.ToString()) as TAttr;

    internal static string TryConvertName(this JsonNamingPolicy? policy, string value) =>
        policy is null ? value : policy.ConvertName(value);

    /// <summary>
    /// Get description from EnumMember.Value Attribute
    /// </summary>
    public static string? GetEnumMemberValue(object @enum, JsonNamingPolicy? namingPolicy = null) =>
        GetAttribute(@enum.GetType(), typeof(EnumMemberAttribute), @enum.ToString()) is
            EnumMemberAttribute
        {
            Value: { } description,
        }
            ? description
            : namingPolicy.TryConvertName(@enum.ToString() ?? string.Empty);

    /// <summary>
    /// Get enum description from EnumMember.Value Attribute
    /// </summary>
    /// <param name="enum"></param>
    /// <param name="namingPolicy"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetEnumMemberValue<T>(this T @enum, JsonNamingPolicy? namingPolicy = null)
        where T : Enum =>
        @enum
            .GetAttribute<T, EnumMemberAttribute>() is { Value: { } description }
            ? description
            : namingPolicy.TryConvertName(@enum.ToString());

    /// <summary>
    /// Get enum description from DescriptionAttribute
    /// </summary>
    /// <param name="enum"></param>
    /// <param name="namingPolicy"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetDescription<T>(this T @enum, JsonNamingPolicy? namingPolicy = null)
        where T : Enum =>
        @enum.GetAttribute<T, DescriptionAttribute>() is { Description: { } description }
            ? description
            : namingPolicy.TryConvertName(@enum.ToString());

    /// <summary>
    /// Get description from DescriptionAttribute
    /// </summary>
    public static string GetDescription(object @enum, JsonNamingPolicy? namingPolicy = null) =>
        GetAttribute(@enum.GetType(), typeof(DescriptionAttribute), @enum.ToString()) is
            DescriptionAttribute
        {
            Description: { } description,
        }
            ? description
            : namingPolicy.TryConvertName(@enum.ToString() ?? string.Empty);

    static TEnum? GetEnumByString<TEnum>(
        string enumDescription,
        Func<TEnum, string?> getString,
        JsonNamingPolicy? namingPolicy = null,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase
    )
        where TEnum : struct, Enum =>
        Enum.GetValues<TEnum>()
            .Select(e =>
                (Value: e, Desc: getString(e) ?? namingPolicy.TryConvertName(e.ToString())))
            .Where(x => string.Equals(x.Desc, enumDescription, comparison))
            .Select(x => x.Value)
            .FirstOrDefault();

    /// <summary>
    /// Return Enum value by description attribute
    /// </summary>
    /// <param name="enumDescription"></param>
    /// <param name="namingPolicy"></param>
    /// <param name="comparison"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum? GetEnumFromDescription<TEnum>(
        string enumDescription,
        JsonNamingPolicy? namingPolicy = null,
        StringComparison comparison = StringComparison.Ordinal)
        where TEnum : struct, Enum =>
        GetEnumByString<TEnum>(enumDescription, e => e.GetDescription(), namingPolicy, comparison);

    /// <summary>
    /// Return Enum value by EnumMember attribute value
    /// </summary>
    /// <param name="enumDescription"></param>
    /// <param name="namingPolicy"></param>
    /// <param name="comparison"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static TEnum? GetEnumFromEnumMemberValue<TEnum>(
        string enumDescription,
        JsonNamingPolicy? namingPolicy = null,
        StringComparison comparison = StringComparison.Ordinal)
        where TEnum : struct, Enum =>
        GetEnumByString<TEnum>(enumDescription, e => e.GetEnumMemberValue(), namingPolicy,
            comparison);

    /// <summary>
    /// Retrieves an enumerable of the values of the constants in a specified enumeration.
    /// </summary>
    public static IEnumerable<object> GetValues(Type enumType) =>
        Enum.GetValues(enumType).Cast<object>();
}
