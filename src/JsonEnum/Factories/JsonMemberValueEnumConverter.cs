using JsonEnum.Converters;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonMemberValueEnumConverter : JsonCustomStringEnumConverterFactory
{
    /// <inheritdoc />
    public JsonMemberValueEnumConverter(StringComparison comparison) : base(comparison) { }

    /// <inheritdoc />
    public JsonMemberValueEnumConverter() :
        base(StringComparison.Ordinal)
    { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonMemberValueEnumConverter<>)
            .MakeGenericType(typeToConvert);
}
