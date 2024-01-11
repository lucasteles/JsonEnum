using System.Text.Json;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

/// <summary>
/// Converter for enums as json using description attribute
/// </summary>
class JsonEnumMemberValueTypeConverter<TEnum> : JsonEnumStringBaseConverter<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    public JsonEnumMemberValueTypeConverter(JsonEnumOptions? options = null) : base(options) { }

    /// <inheritdoc />
    protected override string? GetCustomString(TEnum value, JsonNamingPolicy? policy) =>
        value.GetEnumMemberValue(policy, currentOptions.FlagsValueSeparator);
}
