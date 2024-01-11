using JsonEnum.Converters;
using JsonEnum.Factories.Abstraction;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

using System;

/// <summary>
/// Converter for enums as json using description attribute
/// </summary>
public sealed class JsonEnumNumericConverter : JsonEnumConverterFactory
{
    /// <inheritdoc />
    protected override Type GetCustomConverter(Type typeToConvert) =>
        typeof(JsonEnumNumericValueTypeConverter<>).MakeGenericType(typeToConvert);
}

/// <summary>
/// Converter for enums as json using description attribute
/// </summary>
public sealed class JsonEnumNumericConverter<T> : JsonConverterFactory where T : struct, Enum
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum && typeToConvert == typeof(T);

    /// <inheritdoc />
    public override JsonConverter CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options
    ) => new JsonEnumNumericValueTypeConverter<T>();
}
