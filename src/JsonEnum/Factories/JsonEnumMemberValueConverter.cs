using JsonEnum;
using JsonEnum.Converters;
using JsonEnum.Converters.Abstraction;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

/// <summary>
/// Converter for enums as json using description attribute
/// </summary>
public sealed class JsonEnumMemberValueConverter : JsonEnumConverterFactory
{
    /// <inheritdoc />
    public JsonEnumMemberValueConverter(JsonEnumOptions? options) : base(options) { }

    /// <inheritdoc />
    public JsonEnumMemberValueConverter(JsonNamingPolicy policy) : base(new()
    {
        NamingPolicy = policy,
    })
    { }

    /// <inheritdoc />
    public JsonEnumMemberValueConverter() { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumMemberValueTypeConverter<>).MakeGenericType(typeToConvert);
}

/// <summary>
/// Converter for enums as json using description attribute
/// </summary>
public sealed class JsonEnumMemberValueConverter<TEnum> : JsonEnumStringConverterFactory<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    public JsonEnumMemberValueConverter(JsonEnumOptions? options) : base(options) { }

    /// <inheritdoc />
    public JsonEnumMemberValueConverter(JsonNamingPolicy policy) : base(new()
    {
        NamingPolicy = policy,
    })
    { }

    /// <inheritdoc />
    public JsonEnumMemberValueConverter() { }

    /// <inheritdoc />
    protected override JsonEnumStringBaseConverter<TEnum> GetCustomConverter() =>
        new JsonEnumMemberValueTypeConverter<TEnum>(jsonEnumOptions);
}
