using System.Text.Json;

namespace JsonEnum;

/// <summary>
/// Enum extensions
/// </summary>
public static partial class JsonEnum
{
    /// <summary>
    /// Get enum description from EnumMember.Value Attribute
    /// </summary>
    public static string GetEnumMemberValue<T>(this T @enum,
        JsonNamingPolicy? namingPolicy = null,
        string? flagsSeparator = null
    ) where T : struct, Enum =>
        GetCustomName(@enum, CustomNameSource.EnumMember, namingPolicy, flagsSeparator);

    /// <summary>
    /// Get enum description from DescriptionAttribute
    /// </summary>
    public static string GetDescription<T>(this T @enum,
        JsonNamingPolicy? namingPolicy = null,
        string? flagsSeparator = null
    ) where T : struct, Enum =>
        GetCustomName(@enum, CustomNameSource.Description, namingPolicy, flagsSeparator);

    /// <summary>
    /// Return Enum value by description attribute
    /// </summary>
    public static TEnum? GetEnumFromDescription<TEnum>(
        string enumDescription,
        JsonNamingPolicy? namingPolicy = null,
        StringComparison comparison = StringComparison.Ordinal)
        where TEnum : struct, Enum =>
        GetEnumByString<TEnum>(enumDescription, e => e.GetDescription(), namingPolicy, comparison);

    /// <summary>
    /// Return Enum value by EnumMember attribute value
    /// </summary>
    public static TEnum? GetEnumFromEnumMemberValue<TEnum>(
        string enumDescription,
        JsonNamingPolicy? namingPolicy = null,
        StringComparison comparison = StringComparison.Ordinal)
        where TEnum : struct, Enum =>
        GetEnumByString<TEnum>(enumDescription, e => e.GetEnumMemberValue(), namingPolicy,
            comparison);
}
