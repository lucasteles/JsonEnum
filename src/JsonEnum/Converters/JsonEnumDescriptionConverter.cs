using System.Text.Json;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Converters;

class JsonEnumDescriptionConverter<TEnum> : JsonEnumCustomStringConverter<TEnum>
    where TEnum : struct, Enum
{
    public JsonEnumDescriptionConverter(
        StringComparison comparison = StringComparison.Ordinal,
        JsonNamingPolicy? namingPolicy = null
    ) :
        base(comparison, namingPolicy)
    {
    }

    protected override string GetCustomString(TEnum value) =>
        value.GetDescription(namingPolicy);
}
