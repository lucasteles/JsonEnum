using System.Text.Encodings.Web;
using System.Text.Json;

namespace JsonEnum;

/// <summary>
/// Json Enum parsing options
/// </summary>
public sealed class JsonEnumOptions
{
    const string DefaultEmptyFlagsArray = "None";

    /// <summary>
    /// Gets or sets the comparison used for reading enum values
    /// default: StringComparison.Ordinal
    /// </summary>
    public StringComparison Comparison { get; set; } = StringComparison.Ordinal;

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
    public bool AllowIntegerValues { get; set; }

    /// <summary>
    /// Get or sets the source of the naming policy for JsonSerializerOptions
    /// default: DictionaryKeyPolicy
    /// </summary>
    public JsonOptionsNamingPolicy JsonOptionsNamingPolicySource { get; set; } =
        JsonOptionsNamingPolicy.DictionaryKeyPolicy;


    /// <summary>
    /// Force to use the JsonConverter on the type attribute if present
    /// default: true
    /// </summary>
    public bool PreferTypeConverter { get; set; } = true;

    /// <summary>
    /// Gets or sets enum name to be considered empty array when FlagsAsArray
    /// The case must be 0
    /// default: None
    /// </summary>
    internal string FlagsEmptyName { get; set; } = DefaultEmptyFlagsArray;

    /// <summary>
    /// When true the enum Flags will be serialized as json array
    /// Default: true
    /// </summary>
    internal bool FlagsAsArray { get; set; } = true;

    /// <summary>
    /// Defines which JsonPolicyName property from JsonSerializerOptions will be used
    /// </summary>
    public enum JsonOptionsNamingPolicy : short
    {
        /// <summary>
        /// Use JsonSerializerOptions.DictionaryKeyPolicy
        /// </summary>
        DictionaryKeyPolicy = 1,

        /// <summary>
        /// Use JsonSerializerOptions.PropertyNamingPolicy
        /// </summary>
        PropertyNamingPolicy = 2,
    }

    internal JsonNamingPolicy? GetPolicy(JsonSerializerOptions? jsonOptions = null) =>
        NamingPolicy ?? jsonOptions.GetPolicy(JsonOptionsNamingPolicySource);

    internal JavaScriptEncoder? GetEncoder(JsonSerializerOptions? jsonOptions = null) =>
        Encoder ?? jsonOptions?.Encoder;

    internal JsonEnumOptions Enrich(JsonSerializerOptions options) => new()
    {
        Comparison = Comparison,
        JsonOptionsNamingPolicySource = JsonOptionsNamingPolicySource,
        NamingPolicy = NamingPolicy ?? options.GetPolicy(JsonOptionsNamingPolicySource),
        Encoder = Encoder ?? options.Encoder,
        FlagsValueSeparator = FlagsValueSeparator,
        FlagsAsArray = FlagsAsArray,
        AllowIntegerValues = AllowIntegerValues,
    };
}
