using System.Text.Json;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

/// <summary>
/// Converter for enums as json using string member representation
/// </summary>
class JsonEnumStringTypeConverter<TEnum> : JsonEnumStringBaseConverter<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    protected override bool allowIntegerValues { get; set; } = true;

    /// <inheritdoc />
    public JsonEnumStringTypeConverter(JsonEnumOptions? options = null) : base(options) { }

    /// <inheritdoc />
    protected override string? GetCustomString(TEnum value, JsonNamingPolicy? policy)
    {
        var original = value.GetString();

        if (currentOptions.FlagsValueSeparator is null
            || currentOptions.FlagsValueSeparator == JsonEnum.DefaultValueSeparator
            || !original.Contains(JsonEnum.DefaultValueSeparator))
            return policy.ConvertString(original);

        var values = original.Split(JsonEnum.DefaultValueSeparator);
        for (var i = 0; i < values.Length; i++)
            values[i] = policy.ConvertString(values[i]);
        return string.Join(currentOptions.FlagsValueSeparator, values);
    }
}
