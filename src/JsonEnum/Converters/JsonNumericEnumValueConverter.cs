using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonEnum.Converters;

class JsonEnumNumericValueConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    static readonly TypeCode typeCode = Type.GetTypeCode(typeof(TEnum));


    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        switch (typeCode)
        {
            case TypeCode.Int32:
                writer.WriteNumberValue(Unsafe.As<TEnum, int>(ref value));
                break;
            case TypeCode.UInt32:
                writer.WriteNumberValue(Unsafe.As<TEnum, uint>(ref value));
                break;
            case TypeCode.UInt64:
                writer.WriteNumberValue(Unsafe.As<TEnum, ulong>(ref value));
                break;
            case TypeCode.Int64:
                writer.WriteNumberValue(Unsafe.As<TEnum, long>(ref value));
                break;
            case TypeCode.Int16:
                writer.WriteNumberValue(Unsafe.As<TEnum, short>(ref value));
                break;
            case TypeCode.UInt16:
                writer.WriteNumberValue(Unsafe.As<TEnum, ushort>(ref value));
                break;
            case TypeCode.Byte:
                writer.WriteNumberValue(Unsafe.As<TEnum, byte>(ref value));
                break;
            case TypeCode.SByte:
                writer.WriteNumberValue(Unsafe.As<TEnum, sbyte>(ref value));
                break;
            default:
                throw new JsonException();
        }
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return typeCode switch
                {
                    // Switch cases ordered by expected frequency
                    TypeCode.Int32 when reader.TryGetInt32(out int int32) =>
                        Unsafe.As<int, TEnum>(ref int32),
                    TypeCode.UInt32 when reader.TryGetUInt32(out uint uint32) =>
                        Unsafe.As<uint, TEnum>(ref uint32),
                    TypeCode.UInt64 when reader.TryGetUInt64(out ulong uint64) =>
                        Unsafe.As<ulong, TEnum>(ref uint64),
                    TypeCode.Int64 when reader.TryGetInt64(out long int64) =>
                        Unsafe.As<long, TEnum>(ref int64),
                    TypeCode.SByte when reader.TryGetSByte(out sbyte byte8) =>
                        Unsafe.As<sbyte, TEnum>(ref byte8),
                    TypeCode.Byte when reader.TryGetByte(out byte ubyte8) =>
                        Unsafe.As<byte, TEnum>(ref ubyte8),
                    TypeCode.Int16 when reader.TryGetInt16(out short int16) =>
                        Unsafe.As<short, TEnum>(ref int16),
                    TypeCode.UInt16 when reader.TryGetUInt16(out ushort uint16) =>
                        Unsafe.As<ushort, TEnum>(ref uint16),
                    _ => throw new JsonException(),
                };

            case JsonTokenType.String:
                var str = reader.GetString();
                // Switch cases ordered by expected frequency
                return typeCode switch
                {
                    // Switch cases ordered by expected frequency
                    TypeCode.Int32 when int.TryParse(str, out int int32) =>
                        Unsafe.As<int, TEnum>(ref int32),
                    TypeCode.UInt32 when uint.TryParse(str, out uint uint32) =>
                        Unsafe.As<uint, TEnum>(ref uint32),
                    TypeCode.UInt64 when ulong.TryParse(str, out ulong uint64) =>
                        Unsafe.As<ulong, TEnum>(ref uint64),
                    TypeCode.Int64 when long.TryParse(str, out long int64) =>
                        Unsafe.As<long, TEnum>(ref int64),
                    TypeCode.SByte when sbyte.TryParse(str, out sbyte byte8) =>
                        Unsafe.As<sbyte, TEnum>(ref byte8),
                    TypeCode.Byte when byte.TryParse(str, out byte ubyte8) =>
                        Unsafe.As<byte, TEnum>(ref ubyte8),
                    TypeCode.Int16 when short.TryParse(str, out short int16) =>
                        Unsafe.As<short, TEnum>(ref int16),
                    TypeCode.UInt16 when ushort.TryParse(str, out ushort uint16) =>
                        Unsafe.As<ushort, TEnum>(ref uint16),
                    _ => throw new JsonException(),
                };

            default:
                throw new JsonException("Invalid enum description");
        }
    }
}
