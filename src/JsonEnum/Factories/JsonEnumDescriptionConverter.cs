using JsonEnum.Converters;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonEnumDescriptionConverter : JsonEnumCustomStringConverterFactory
{
    /// <inheritdoc />
    public JsonEnumDescriptionConverter(StringComparison comparison,
        JsonNamingPolicy? namingPolicy = null)
        :
        base(comparison, namingPolicy)
    {
    }

    /// <inheritdoc />
    public JsonEnumDescriptionConverter() :
        base(StringComparison.Ordinal)
    {
    }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumDescriptionConverter<>)
            .MakeGenericType(typeToConvert);
}
