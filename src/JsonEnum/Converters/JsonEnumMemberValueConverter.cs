using System.Text.Json;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

class JsonMemberValueEnumConverter<TEnum> : JsonCustomStringEnumConverter<TEnum>
    where TEnum : struct, Enum
{
    public JsonMemberValueEnumConverter(
        StringComparison comparison = StringComparison.Ordinal,
        JsonNamingPolicy? namingPolicy = null
    ) :
        base(comparison, namingPolicy)
    {
    }

    protected override string GetCustomString(TEnum value, JsonNamingPolicy? namingPolicy) =>
        value.GetEnumMemberValue(namingPolicy);
}
