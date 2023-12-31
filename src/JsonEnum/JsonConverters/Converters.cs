using System;

namespace JsonEnum.JsonConverters;

class JsonEnumMemberValueConverter<TEnum> : JsonEnumCustomStringConverter<TEnum>
    where TEnum : struct, Enum
{
    public JsonEnumMemberValueConverter(StringComparison comparison = StringComparison.Ordinal) :
        base(comparison)
    { }

    protected override string GetCustomString(TEnum value) => value.GetEnumMemberValue();

    protected override TEnum? GetValueFromString(string value, StringComparison comparison) =>
        EnumHelpers.GetEnumFromEnumMemberValue<TEnum>(value, comparison);
}

class JsonEnumDescriptionConverter<TEnum> : JsonEnumCustomStringConverter<TEnum>
    where TEnum : struct, Enum
{
    public JsonEnumDescriptionConverter(StringComparison comparison = StringComparison.Ordinal) :
        base(comparison)
    { }

    protected override string GetCustomString(TEnum value) => value.GetDescription();

    protected override TEnum? GetValueFromString(string value, StringComparison comparison) =>
        EnumHelpers.GetEnumFromDescription<TEnum>(value, comparison);
}

class JsonEnumNumericStringConverter<TEnum> : JsonEnumCustomStringConverter<TEnum>
    where TEnum : struct, Enum
{
    public JsonEnumNumericStringConverter(StringComparison comparison = StringComparison.Ordinal)
        :
        base(comparison)
    { }

    protected override string GetCustomString(TEnum value) =>
        value.ToString("d");

    protected override TEnum? GetValueFromString(string value, StringComparison comparison) =>
        Enum.Parse<TEnum>(value);
}
