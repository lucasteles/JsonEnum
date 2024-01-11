using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonEnum.Factories.Abstraction;

/// <summary>
/// Base JsonEnum converter attribute
/// </summary>
public abstract class JsonEnumConverterAttribute : JsonConverterAttribute
{
    /// <summary>
    /// Gets or sets the name policy for enum value serialization
    /// </summary>
    public JsonNamingPolicy? NamingPolicy { get; set; }

    /// <summary>
    /// Gets or sets the encoder to use when escaping strings, or null to use the default encoder.
    /// </summary>
    public JavaScriptEncoder? Encoder { get; set; }

    /// <summary>
    /// Gets or sets separator for string serialization of Flags
    /// </summary>
    public string? FlagsValueSeparator { get; set; }

    /// <summary>
    /// Allow parsing from integer values
    /// </summary>
    public bool? AllowIntegerValues { get; set; }

    /// <summary>
    /// Get or sets the source of the naming policy for JsonSerializerOptions
    /// default: DictionaryKeyPolicy
    /// </summary>
    public JsonEnumOptions.JsonOptionsNamingPolicy? JsonOptionsNamingPolicySource { get; set; }

    /// <summary>
    /// Gets or sets the comparison used for reading enum values
    /// default: StringComparison.Ordinal
    /// </summary>
    public StringComparison? Comparison { get; set; }

    /// <summary>
    /// When true the enum Flags will be serialized as json array
    /// Default: true
    /// </summary>
    public bool? FlagsAsArray { get; set; }

    /// <summary>
    /// Gets or sets enum name to be considered empty array when FlagsAsArray
    /// The case must be 0
    /// default: None
    /// </summary>
    public string? FlagsEmptyName { get; set; }

    internal JsonEnumOptions GetOptions()
    {
        JsonEnumOptions options = new()
        {
            NamingPolicy = NamingPolicy,
            Encoder = Encoder,
            FlagsValueSeparator = FlagsValueSeparator,
        };

        if (Comparison is not null)
            options.Comparison = Comparison.Value;

        if (FlagsAsArray is not null)
            options.FlagsAsArray = FlagsAsArray.Value;

        if (AllowIntegerValues is not null)
            options.AllowIntegerValues = AllowIntegerValues.Value;

        if (JsonOptionsNamingPolicySource is not null)
            options.JsonOptionsNamingPolicySource = JsonOptionsNamingPolicySource.Value;

        return options;
    }
}
