using System.Text.Json;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

/// <summary>
/// Converter for enums as json using description attribute
/// </summary>
class JsonEnumDescriptionTypeConverter<TEnum> : JsonEnumStringBaseConverter<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    public JsonEnumDescriptionTypeConverter(JsonEnumOptions? options = null) : base(options) { }

    /// <inheritdoc />
    protected override string? GetCustomString(TEnum value, JsonNamingPolicy? policy) =>
        value.GetDescription(policy, currentOptions.FlagsValueSeparator);
}
