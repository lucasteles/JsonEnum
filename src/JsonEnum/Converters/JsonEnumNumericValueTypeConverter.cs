using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonEnum.Converters;

/// <summary>
/// Converter for enums as json enums using integer string representation
/// </summary>
class JsonEnumNumericValueTypeConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    static readonly TypeCode typeCode = Type.GetTypeCode(typeof(TEnum));

    public JsonEnumNumericValueTypeConverter(JsonEnumOptions? options) { }
    public JsonEnumNumericValueTypeConverter() : this(null) { }

    /// <inheritdoc />
    public sealed override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsEnum && typeToConvert == typeof(TEnum);

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return JsonEnum.JsonReadEnumNumber<TEnum>(ref reader, typeCode);

            case JsonTokenType.String when JsonEnum.TryConvertFromString(reader.GetString(), typeCode, out TEnum value):
                return value;

            default:
                throw new JsonException("Invalid enum description");
        }
    }
}
