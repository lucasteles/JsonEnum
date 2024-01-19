using System.Runtime.CompilerServices;
using System.Text.Json;

namespace JsonEnum;

static class ExtensionsInternal
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ConvertString(this JsonNamingPolicy? namePolicy, string name) =>
        namePolicy is null ? name : namePolicy.ConvertName(name);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static JsonNamingPolicy? GetPolicy(
        this JsonSerializerOptions? jsonOptions,
        JsonEnumOptions.JsonOptionsNamingPolicy source
    ) =>
        source switch
        {
            JsonEnumOptions.JsonOptionsNamingPolicy.DictionaryKeyPolicy => jsonOptions?.DictionaryKeyPolicy,
            JsonEnumOptions.JsonOptionsNamingPolicy.PropertyNamingPolicy => jsonOptions?.PropertyNamingPolicy,
            _ => null,
        };

    public static object? GetFieldAttribute(this Type valueType, string? fieldName, Type attributeType)
    {
        if (string.IsNullOrWhiteSpace(fieldName)) return null;

        return valueType.GetField(fieldName) is { } member
            ? Attribute.GetCustomAttribute(member, attributeType, false)
            : null;
    }

    public static string GetString<T>(this T value) where T : struct, Enum =>
        Enum.GetName(value) ?? value.ToString();
}

/// <summary>
/// JsonEnum Extensions
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Gets the equivalent JsonNamingPolicy
    /// </summary>
    public static JsonNamingPolicy? ToJsonNamingPolicy(this JsonEnumNamingPolicy policy) =>
        policy switch
        {
            JsonEnumNamingPolicy.CamelCase => JsonNamingPolicy.CamelCase,
#if NET8_0_OR_GREATER
            JsonEnumNamingPolicy.SnakeCaseLower => JsonNamingPolicy.SnakeCaseLower,
            JsonEnumNamingPolicy.SnakeCaseUpper => JsonNamingPolicy.SnakeCaseUpper,
            JsonEnumNamingPolicy.KebabCaseLower => JsonNamingPolicy.KebabCaseLower,
            JsonEnumNamingPolicy.KebabCaseUpper => JsonNamingPolicy.KebabCaseUpper,
#endif
            _ => null,
        };
}
