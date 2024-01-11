using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

class JsonEnumNumericStringConverter<TEnum> : JsonEnumCustomStringConverter<TEnum>
    where TEnum : struct, Enum
{
    public override bool AllowNumbers { get; set; } = true;

    public JsonEnumNumericStringConverter(StringComparison comparison = StringComparison.Ordinal)
        : base(comparison)
    {
    }

    protected override string GetCustomString(TEnum value) =>
        value.ToString("d");
}
