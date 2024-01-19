using JsonEnum;
using JsonEnum.Converters;
using JsonEnum.Converters.Abstraction;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for enums as json enums using string member representation
/// </summary>
public sealed class JsonEnumStringConverter : JsonEnumConverterFactory
{
    /// <inheritdoc />
    public JsonEnumStringConverter(JsonEnumOptions? options) : base(options) { }

    /// <inheritdoc />
    public JsonEnumStringConverter(JsonNamingPolicy? policy) : base(new()
    {
        NamingPolicy = policy,
    })
    { }

    /// <inheritdoc />
    public JsonEnumStringConverter() { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumStringTypeConverter<>).MakeGenericType(typeToConvert);
}

/// <summary>
/// Converter for enums as json enums using string member representation
/// </summary>
public sealed class JsonEnumStringConverter<TEnum> : JsonEnumStringConverterFactory<TEnum> where TEnum : struct, Enum
{
    /// <inheritdoc />
    public JsonEnumStringConverter(JsonEnumOptions? options) : base(options) { }

    /// <inheritdoc />
    public JsonEnumStringConverter(JsonNamingPolicy policy) : base(new()
    {
        NamingPolicy = policy,
    })
    { }

    /// <inheritdoc />
    public JsonEnumStringConverter() { }

    /// <inheritdoc />
    protected override JsonEnumStringBaseConverter<TEnum> GetCustomConverter() =>
        new JsonEnumStringTypeConverter<TEnum>(jsonEnumOptions);
}
