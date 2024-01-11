using System;
using System.Text.Json;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

class JsonEnumMemberValueConverter<TEnum> : JsonEnumCustomStringConverter<TEnum>
    where TEnum : struct, Enum
{
    public JsonEnumMemberValueConverter(
        StringComparison comparison = StringComparison.Ordinal,
        JsonNamingPolicy? namingPolicy = null
    ) :
        base(comparison, namingPolicy)
    {
    }

    protected override string GetCustomString(TEnum value) =>
        value.GetEnumMemberValue(namingPolicy);
}
