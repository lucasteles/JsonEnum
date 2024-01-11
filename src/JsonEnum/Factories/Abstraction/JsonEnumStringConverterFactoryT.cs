using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonEnum.Converters.Abstraction;

namespace JsonEnum.Factories.Abstraction;

/// <summary>
/// base string enum converter
/// </summary>
public abstract class JsonEnumStringConverterFactory<TEnum> : JsonConverterFactory where TEnum : struct, Enum
{
    /// <summary>
    /// JsonEnum Options
    /// </summary>
    protected JsonEnumOptions? jsonEnumOptions;

    /// <inheritdoc />
    protected JsonEnumStringConverterFactory(JsonEnumOptions? jsonEnumOptions = null)
    {
        this.jsonEnumOptions = jsonEnumOptions;
        WithOptions(jsonEnumOptions);
    }

    internal JsonEnumStringConverterFactory<TEnum> WithOptions(JsonEnumOptions? options)
    {
        jsonEnumOptions = options;
        return this;
    }

    /// <inheritdoc />
    public sealed override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsEnum && typeToConvert == typeof(TEnum);

    /// <summary>
    /// Converter Type Factory
    /// </summary>
    /// <returns></returns>
    protected abstract JsonEnumStringBaseConverter<TEnum> GetCustomConverter();

    /// <inheritdoc />
    public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonEnumConverter = GetCustomConverter();

        if (jsonEnumOptions is not null && !jsonEnumOptions.PreferTypeConverter)
            return jsonEnumConverter;

        var jsonEnumConverterType = jsonEnumConverter.GetType();
        var converterAttrType = typeToConvert
            .GetCustomAttributes<JsonConverterAttribute>(true)
            .Select(x => x.ConverterType)
            .Where(x => x is not null && x != GetType() && x != jsonEnumConverterType)
            .Cast<Type>()
            .LastOrDefault();

        if (converterAttrType is null)
            return jsonEnumConverter;

        return Activator.CreateInstance(converterAttrType) switch
        {
            JsonEnumStringConverterFactory<TEnum> enumStringFactoryT =>
                enumStringFactoryT.WithOptions(jsonEnumOptions?.Enrich(options)),
            JsonEnumConverterFactory enumStringFactory =>
                enumStringFactory.WithOptions(jsonEnumOptions?.Enrich(options)),
            JsonConverterFactory factory
                when factory.CreateConverter(typeToConvert, options) is { } converter => converter,
            JsonConverter converter => converter,
            _ => throw new InvalidOperationException(),
        };
    }
}
