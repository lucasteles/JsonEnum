using System.Text.Json;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

class JsonNumericStringEnumConverter<TEnum> : JsonCustomStringEnumConverter<TEnum>
    where TEnum : struct, Enum
{
    public override bool AllowNumbers { get; set; } = true;

    public JsonNumericStringEnumConverter(StringComparison comparison = StringComparison.Ordinal)
        : base(comparison) { }

    protected override string GetCustomString(TEnum value, JsonNamingPolicy? namingPolicy) =>
        value.ToString("d");
}
