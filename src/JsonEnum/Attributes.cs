// ReSharper disable once CheckNamespace

using JsonEnum.Factories.Abstraction;

namespace System.Text.Json.Serialization;

/// <summary>
/// Sets enum to use default string value for json serialization
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct |
    AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class JsonEnumStringAttribute : JsonEnumConverterAttribute
{
    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert) =>
        new JsonEnumStringConverter(GetOptions());
}

/// <summary>
/// Sets enum to use description for json serialization
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct |
    AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class JsonEnumDescriptionAttribute : JsonEnumConverterAttribute
{
    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert) =>
        new JsonEnumDescriptionConverter(GetOptions());
}

/// <summary>
/// Sets enum to use enum member value for json serialization
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct |
    AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class JsonEnumMemberValueAttribute : JsonEnumConverterAttribute
{
    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert) =>
        new JsonEnumMemberValueConverter(GetOptions());
}

/// <summary>
/// Sets enum to use string numeric values, for json serialization
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct |
    AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class JsonEnumNumericStringAttribute : JsonEnumConverterAttribute
{
    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert) =>
        new JsonEnumNumericStringConverter(GetOptions());
}

/// <summary>
/// Sets enum to use numeric values for json serialization
/// </summary>
[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct |
    AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
public sealed class JsonEnumNumericAttribute : JsonConverterAttribute
{
    /// <inheritdoc />
    public JsonEnumNumericAttribute() : base(typeof(JsonEnumNumericConverter)) { }
}
