using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonEnum.Factories.Abstraction;

/// <summary>
/// base string enum converter
/// </summary>
public abstract class JsonEnumConverterFactory : JsonConverterFactory
{
    JsonEnumOptions? enumOptions;
    object?[] converterTypeArgs = Array.Empty<object>();

    /// <inheritdoc />
    protected JsonEnumConverterFactory(JsonEnumOptions? enumOptions = null)
    {
        this.enumOptions = enumOptions;
        WithOptions(enumOptions);
    }

    internal JsonEnumConverterFactory WithOptions(JsonEnumOptions? options)
    {
        enumOptions = options;
        converterTypeArgs = new[]
        {
            options,
        };
        return this;
    }

    /// <inheritdoc />
    public sealed override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    /// <summary>
    /// Converter Type Factory
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <returns></returns>
    protected abstract Type GetCustomConverter(Type typeToConvert);


    /// <inheritdoc />
    public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = GetCustomConverter(typeToConvert);

        if (enumOptions is not null && !enumOptions.PreferTypeConverter)
            return (JsonConverter)Activator.CreateInstance(converterType, converterTypeArgs)!;

        var attrConverter = typeToConvert
            .GetCustomAttributes<JsonConverterAttribute>(true)
            .LastOrDefault();

        if (attrConverter is null)
            return InstantiateConverter();

        var typeConverter = attrConverter.ConverterType is not null
            ? (JsonConverter?)Activator.CreateInstance(attrConverter.ConverterType)
            : attrConverter.CreateConverter(typeToConvert);

        if (typeConverter is null || typeConverter.GetType() == converterType || typeConverter.GetType() == GetType())
            return InstantiateConverter();

        return typeConverter switch
        {
            JsonEnumConverterFactory jsonEnumFactory
                when jsonEnumFactory
                    .WithOptions(enumOptions?.Enrich(options))
                    .CreateConverter(typeToConvert, options) is { } converter =>
                converter,
            JsonConverterFactory factory
                when factory.CreateConverter(typeToConvert, options) is { } converter => converter,
            _ => InstantiateConverter(),
        };

        JsonConverter InstantiateConverter() =>
            (JsonConverter)Activator.CreateInstance(converterType, converterTypeArgs)!;
    }
}
