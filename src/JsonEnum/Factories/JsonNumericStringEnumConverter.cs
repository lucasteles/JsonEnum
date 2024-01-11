using JsonEnum.Converters;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonNumericStringEnumConverter : JsonCustomStringEnumConverterFactory
{
    readonly object?[] converterTypeArgs;

    /// <inheritdoc />
    public JsonNumericStringEnumConverter() : base(StringComparison.InvariantCultureIgnoreCase) =>
        converterTypeArgs = new object[]
        {
            stringComparison,
        };

    /// <inheritdoc />
    protected override object?[] CustomConverterArgs() => converterTypeArgs;

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonNumericStringEnumConverter<>)
            .MakeGenericType(typeToConvert);
}
