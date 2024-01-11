using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonEnum.Converters.Abstraction;

abstract class JsonEnumCustomStringConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    protected readonly JsonNamingPolicy? namingPolicy;

    readonly Dictionary<TEnum, string> nameCache;
    readonly Dictionary<string, TEnum> valueCache;

    static readonly TypeCode typeCode = Type.GetTypeCode(typeof(TEnum));

    protected abstract string GetCustomString(TEnum value);

    public virtual bool AllowNumbers { get; set; } = false;

    protected JsonEnumCustomStringConverter(
        StringComparison comparison = StringComparison.Ordinal,
        JsonNamingPolicy? namingPolicy = null
    )
    {
        this.namingPolicy = namingPolicy;

        nameCache = new();
        valueCache = new(StringComparer.FromComparison(comparison));

        LoadCache();
    }

    void LoadCache()
    {
        foreach (var value in Enum.GetValues<TEnum>())
        {
            var name = GetCustomString(value);
            nameCache.Add(value, name);
            valueCache.Add(name, value);
        }
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options) =>
        writer.WriteStringValue(nameCache.GetValueOrDefault(value));

    public override TEnum Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.Number when AllowNumbers => typeCode switch
            {
                // Switch cases ordered by expected frequency
                TypeCode.Int32 when reader.TryGetInt32(out int int32) => Unsafe.As<int, TEnum>(
                    ref int32),
                TypeCode.UInt32 when reader.TryGetUInt32(out uint uint32) => Unsafe.As<uint, TEnum>(
                    ref uint32),
                TypeCode.UInt64 when reader.TryGetUInt64(out ulong uint64) => Unsafe
                    .As<ulong, TEnum>(ref uint64),
                TypeCode.Int64 when reader.TryGetInt64(out long int64) => Unsafe.As<long, TEnum>(
                    ref int64),
                TypeCode.SByte when reader.TryGetSByte(out sbyte byte8) => Unsafe.As<sbyte, TEnum>(
                    ref byte8),
                TypeCode.Byte when reader.TryGetByte(out byte ubyte8) => Unsafe.As<byte, TEnum>(
                    ref ubyte8),
                TypeCode.Int16 when reader.TryGetInt16(out short int16) => Unsafe.As<short, TEnum>(
                    ref int16),
                TypeCode.UInt16 when reader.TryGetUInt16(out ushort uint16) => Unsafe
                    .As<ushort, TEnum>(ref uint16),
                _ => throw new JsonException(),
            },
            JsonTokenType.String when reader.GetString() is { } enumString &&
                                      valueCache.TryGetValue(enumString, out var value) => value,
            _ => throw new JsonException("Invalid enum description"),
        };
}
