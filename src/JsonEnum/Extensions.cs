using System.Runtime.CompilerServices;
using System.Text.Json;

namespace JsonEnum;

static class Extensions
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
