using System.Text.Json;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

/// <summary>
/// Converter for enums as json enums using integer string representation
/// </summary>
class JsonEnumNumericStringTypeConverter<TEnum> : JsonEnumStringBaseConverter<TEnum>
    where TEnum : struct, Enum
{
    static readonly TypeCode typeCode = Type.GetTypeCode(typeof(TEnum));

    /// <summary>
    /// Allow parsing integer value
    /// </summary>
    protected override bool allowIntegerValues { get; set; } = true;

    Dictionary<ulong, string>? valueCache;

    /// <inheritdoc />
    public JsonEnumNumericStringTypeConverter(JsonEnumOptions? options = null) : base(options) { }

    /// <inheritdoc />
    protected override string? GetCustomString(TEnum value, JsonNamingPolicy? policy)
    {
        valueCache ??= Enum.GetValues<TEnum>()
            .Select(v => JsonEnum.ConvertToUInt64(v))
            .ToDictionary(x => x, x => Convert.ToString(x));

        var intValue = JsonEnum.ConvertToUInt64(value);

        if (valueCache.TryGetValue(intValue, out var original))
            return policy.ConvertString(original);

        if (currentOptions.FlagsValueSeparator is null)
            return policy.ConvertString(Convert.ToString(intValue));

        return JsonEnum.GetFlagListString(value, policy,
            currentOptions.FlagsValueSeparator ?? JsonEnum.DefaultValueSeparator);
    }

    protected override TEnum ParseCustomString(string? value) =>
        JsonEnum.TryConvertFromString(value, typeCode, out TEnum result) ? result : default;
}
