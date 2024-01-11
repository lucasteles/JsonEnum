using JsonEnum.Converters;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonDescriptionEnumConverter : JsonCustomStringEnumConverterFactory
{
    /// <inheritdoc />
    public JsonDescriptionEnumConverter(
        StringComparison comparison,
        JsonNamingPolicy? namingPolicy = null
    ) : base(comparison, namingPolicy) { }

    /// <inheritdoc />
    public JsonDescriptionEnumConverter() : base(StringComparison.Ordinal) { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonDescriptionEnumConverter<>)
            .MakeGenericType(typeToConvert);
}
