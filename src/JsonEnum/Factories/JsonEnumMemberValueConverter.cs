using JsonEnum.Converters;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonEnumMemberValueConverter : JsonEnumCustomStringConverterFactory
{
    /// <inheritdoc />
    public JsonEnumMemberValueConverter(StringComparison comparison) :
        base(comparison)
    {
    }

    /// <inheritdoc />
    public JsonEnumMemberValueConverter() :
        base(StringComparison.Ordinal)
    {
    }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumMemberValueConverter<>)
            .MakeGenericType(typeToConvert);
}
