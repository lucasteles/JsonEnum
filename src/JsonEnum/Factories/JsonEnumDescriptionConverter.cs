using JsonEnum;
using JsonEnum.Converters;
using JsonEnum.Converters.Abstraction;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for enums as json using description attribute
/// </summary>
public sealed class JsonEnumDescriptionConverter : JsonEnumConverterFactory
{
    /// <inheritdoc />
    public JsonEnumDescriptionConverter(JsonEnumOptions? options) : base(options) { }

    /// <inheritdoc />
    public JsonEnumDescriptionConverter(JsonNamingPolicy policy) : base(new()
    {
        NamingPolicy = policy,
    })
    { }

    /// <inheritdoc />
    public JsonEnumDescriptionConverter() { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumDescriptionTypeConverter<>).MakeGenericType(typeToConvert);
}

/// <summary>
/// Converter for enums as json using description attribute
/// </summary>
public sealed class JsonEnumDescriptionConverter<TEnum> : JsonEnumStringConverterFactory<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    public JsonEnumDescriptionConverter(JsonEnumOptions? options) : base(options) { }

    /// <inheritdoc />
    public JsonEnumDescriptionConverter(JsonNamingPolicy policy) : base(new()
    {
        NamingPolicy = policy,
    })
    { }

    /// <inheritdoc />
    public JsonEnumDescriptionConverter() { }

    /// <inheritdoc />
    protected override JsonEnumStringBaseConverter<TEnum> GetCustomConverter() =>
        new JsonEnumDescriptionTypeConverter<TEnum>(jsonEnumOptions);
}
