using JsonEnum;
using JsonEnum.Converters;
using JsonEnum.Converters.Abstraction;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for enums as json enums using integer string representation
/// </summary>
public sealed class JsonEnumNumericStringConverter : JsonEnumConverterFactory
{
    /// <inheritdoc />
    public JsonEnumNumericStringConverter(JsonEnumOptions? options) : base(options) { }

    /// <inheritdoc />
    public JsonEnumNumericStringConverter() { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumNumericStringTypeConverter<>).MakeGenericType(typeToConvert);
}

/// <summary>
/// Converter for enums as json enums using integer string representation
/// </summary>
public sealed class JsonEnumNumericStringConverter<TEnum> : JsonEnumStringConverterFactory<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    public JsonEnumNumericStringConverter(JsonEnumOptions? options) : base(options) { }

    /// <inheritdoc />
    public JsonEnumNumericStringConverter() { }

    /// <inheritdoc />
    protected override JsonEnumStringBaseConverter<TEnum> GetCustomConverter() =>
        new JsonEnumNumericStringTypeConverter<TEnum>(jsonEnumOptions);
}
