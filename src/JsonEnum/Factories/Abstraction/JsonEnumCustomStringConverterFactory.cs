using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonEnum.Factories.Abstraction;

/// <summary>
/// base string enum converter
/// </summary>
public abstract class JsonEnumCustomStringConverterFactory : JsonConverterFactory
{
    /// <summary>
    /// String comparison method for enum names
    /// </summary>
    protected readonly StringComparison stringComparison;

    readonly object?[] converterTypeArgs;

    /// <summary>
    /// Construct converter
    /// </summary>
    /// <param name="stringComparison" >
    /// string comparison to determine if the string matches
    /// </param>
    /// <param name="namingPolicy"></param>
    protected JsonEnumCustomStringConverterFactory(
        StringComparison stringComparison,
        JsonNamingPolicy? namingPolicy = null
    )
    {
        this.stringComparison = stringComparison;
        converterTypeArgs = new object?[]
        {
            stringComparison, namingPolicy,
        };
    }

    /// <summary>
    /// Construct converter
    /// uses string comparison to determine if the string matches
    /// </summary>
    protected JsonEnumCustomStringConverterFactory() : this(StringComparison.Ordinal)
    {
        // An empty constructor is needed for construction via attributes
    }

    /// <inheritdoc />
    public sealed override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    /// <summary>
    /// Converter Type Factory
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <returns></returns>
    protected abstract Type GetCustomConverter(Type typeToConvert);

    /// <summary>
    /// Converter Type Arguments
    /// </summary>
    protected virtual object?[] CustomConverterArgs() => converterTypeArgs;

    /// <inheritdoc />
    public sealed override JsonConverter CreateConverter(Type typeToConvert,
        JsonSerializerOptions options) =>
        (JsonConverter)Activator.CreateInstance(
            GetCustomConverter(typeToConvert),
            CustomConverterArgs()
        )!;
}
