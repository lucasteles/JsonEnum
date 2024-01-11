using JsonEnum.Converters;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

using System;

/// <summary>
/// Converter for json using description attribute
/// </summary>
public sealed class JsonNumericEnumConverter : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    /// <inheritdoc />
    public override JsonConverter CreateConverter(
        Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter)Activator.CreateInstance(
            typeof(JsonNumericValueEnumConverter<>)
                .MakeGenericType(typeToConvert))!;
}
