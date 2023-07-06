using JsonEnum.JsonConverters;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

using System;

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonEnumDescriptionConverter : JsonEnumCustomStringConverterFactory
{
    /// <inheritdoc />
    public JsonEnumDescriptionConverter(StringComparison comparison) :
        base(comparison)
    { }

    /// <inheritdoc />
    public JsonEnumDescriptionConverter() :
        base(StringComparison.Ordinal)
    { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumDescriptionConverter<>)
            .MakeGenericType(typeToConvert);
}

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonEnumMemberValueConverter : JsonEnumCustomStringConverterFactory
{
    /// <inheritdoc />
    public JsonEnumMemberValueConverter(StringComparison comparison) :
        base(comparison)
    { }

    /// <inheritdoc />
    public JsonEnumMemberValueConverter() :
        base(StringComparison.Ordinal)
    { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumMemberValueConverter<>)
            .MakeGenericType(typeToConvert);
}

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonEnumNumericStringConverter : JsonEnumCustomStringConverterFactory
{
    /// <inheritdoc />
    public JsonEnumNumericStringConverter() : base(StringComparison.Ordinal) { }

    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumNumericStringConverter<>)
            .MakeGenericType(typeToConvert);
}

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonEnumNumericConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    /// <inheritdoc />
    public override JsonConverter CreateConverter(
        Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter)Activator.CreateInstance(
            typeof(JsonEnumNumericValueConverter<>)
                .MakeGenericType(typeToConvert))!;
}
