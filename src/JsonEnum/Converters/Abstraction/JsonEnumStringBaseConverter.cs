using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonEnum.Converters.Abstraction;

/// <summary>
/// Base class for enum string serialization
/// </summary>
public abstract class JsonEnumStringBaseConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    const int NameCacheSizeSoftLimit = 128;
    static readonly TypeCode typeCode = Type.GetTypeCode(typeof(TEnum));

    readonly ConcurrentDictionary<(ulong, JsonNamingPolicy?), JsonEncodedText> nameCache = new();
    readonly ConcurrentDictionary<string, TEnum> valueCache;
    readonly HashSet<JsonNamingPolicy?> loadedPolicies = new();

    /// <summary>
    /// Current json enum settings
    /// </summary>
    protected readonly JsonEnumOptions currentOptions;
    // ReSharper disable once StaticMemberInGenericType
#pragma warning disable S2743
    static readonly string? negativeSign = ((int)typeCode % 2) == 0 ? null : NumberFormatInfo.CurrentInfo.NegativeSign;
#pragma warning restore S2743

    /// <summary>
    /// Defines how to get the enum string representation
    /// </summary>
    protected abstract string? GetCustomString(TEnum value, JsonNamingPolicy? policy);

    /// <summary>
    /// Allow parsing integer value
    /// </summary>
    protected virtual bool allowIntegerValues { get; set; } = false;

    /// <summary>
    /// Constructs the enum string converter with options
    /// </summary>
    protected JsonEnumStringBaseConverter(JsonEnumOptions? currentOptions)
    {
        currentOptions ??= new();
        this.currentOptions = currentOptions;
        valueCache = new(StringComparer.FromComparison(currentOptions.Comparison));
        LoadCache(null);
    }

    /// <summary>
    /// Defines custom parsing for non default string values
    /// </summary>
    protected virtual TEnum ParseCustomString(string value) => default;

    void LoadCache(JsonSerializerOptions? jsonOptions)
    {
        loadedPolicies.Add(jsonOptions?.DictionaryKeyPolicy);
        foreach (var value in Enum.GetValues<TEnum>())
            AddToCache(value, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    JsonEncodedText AddToCache(TEnum value, JsonSerializerOptions? jsonOptions)
    {
        var policy = currentOptions.GetPolicy(jsonOptions);
        var encoder = currentOptions.GetEncoder(jsonOptions);

        var name = GetCustomString(value, policy) ?? policy.ConvertString(value.GetString());
        var encoded = JsonEncodedText.Encode(name, encoder);

        if (nameCache.Count >= NameCacheSizeSoftLimit)
            return encoded;

        var key = JsonEnum.ConvertToUInt64(value, typeCode);
        valueCache.TryAdd(name, value);
        nameCache.TryAdd((key, policy), encoded);

        return encoded;
    }

    /// <inheritdoc />
    public sealed override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsEnum && typeToConvert == typeof(TEnum);

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (currentOptions.NamingPolicy is null && !loadedPolicies.Contains(options.DictionaryKeyPolicy))
            LoadCache(options);

        var key = JsonEnum.ConvertToUInt64(value, typeCode);
        var policy = currentOptions.GetPolicy(options);

        if (nameCache.TryGetValue((key, policy), out JsonEncodedText formatted))
        {
            writer.WriteStringValue(formatted);
            return;
        }

        var original = value.ToString();
        if (!IsValidIdentifier(original))
            return;

        formatted = AddToCache(value, options);
        writer.WriteStringValue(formatted);
    }

    /// <inheritdoc />
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.Number when allowIntegerValues => JsonEnum.JsonReadEnumNumber<TEnum>(ref reader, typeCode),
            JsonTokenType.String when reader.GetString() is { } enumString => GetEnumValue(enumString, options),
            _ => throw new JsonException("Invalid enum description"),
        };

    TEnum GetEnumValue(string enumString, JsonSerializerOptions jsonOptions)
    {
        if (!loadedPolicies.Contains(jsonOptions.DictionaryKeyPolicy))
            LoadCache(jsonOptions);

        var separator = currentOptions.FlagsValueSeparator ?? JsonEnum.DefaultValueSeparator;
        if (!enumString.Contains(separator))
            return valueCache.TryGetValue(enumString, out var value) ? value : ParseCustomString(enumString);

        var values = enumString.Split(separator);
        ulong flag = 0;

        for (var index = 0; index < values.Length; index++)
        {
            if (!valueCache.TryGetValue(values[index], out var @enum))
                throw new JsonException();

            flag |= JsonEnum.ConvertToUInt64(@enum);
        }

        return JsonEnum.TryConvertFromUInt64<TEnum>(flag, typeCode);
    }

    static bool IsValidIdentifier(string value) =>
        // Trying to do this check efficiently. When an enum is converted to
        // string the underlying value is given if it can't find a matching
        // identifier (or identifiers in the case of flags).
        //
        // The underlying value will be given back with a digit (e.g. 0-9) possibly
        // preceded by a negative sign. Identifiers have to start with a letter
        // so we'll just pick the first valid one and check for a negative sign
        // if needed.
        value[0] >= 'A' && (negativeSign is null || !value.StartsWith(negativeSign));
}
