using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace JsonEnum;

static partial class JsonEnum
{
    internal const string DefaultValueSeparator = ", ";

    static object? GetAttribute(Type enumType, Type attributeType, string? caseName) =>
        !enumType.IsEnum ? null : enumType.GetFieldAttribute(caseName, attributeType);

    internal enum CustomNameSource
    {
        Description,
        EnumMember,
    }

    static string GetCustomName<T>(this T @enum,
        CustomNameSource source,
        JsonNamingPolicy? namingPolicy = null,
        string? flagsSeparator = null
    )
        where T : struct, Enum
    {
        var original = @enum.GetString();

        if (!original.Contains(DefaultValueSeparator))
            return GetString(original);

        var values = original.Split(DefaultValueSeparator);
        for (var i = 0; i < values.Length; i++)
            values[i] = GetString(values[i]);

        return string.Join(flagsSeparator ?? DefaultValueSeparator, values);

        string GetString(string field) =>
            namingPolicy.ConvertString(
                source switch
                {
                    CustomNameSource.Description =>
                        GetAttribute(typeof(T), typeof(DescriptionAttribute), field)
                            is DescriptionAttribute { Description: { } description }
                            ? description
                            : field,
                    CustomNameSource.EnumMember =>
                        GetAttribute(typeof(T), typeof(EnumMemberAttribute), field)
                            is EnumMemberAttribute { Value: { } memberValue }
                            ? memberValue
                            : field,
                    _ => field,
                }
            );
    }

    static TEnum? GetEnumByString<TEnum>(
        string enumDescription,
        Func<TEnum, string?> getString,
        JsonNamingPolicy? namingPolicy = null,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase
    )
        where TEnum : struct, Enum =>
        Enum.GetValues<TEnum>()
            .Select(e => (
                Value: e,
                Desc: getString(e) ?? (
                    namingPolicy is null
                        ? e.GetString()
                        : namingPolicy.ConvertName(e.GetString())
                )
            ))
            .Where(x => string.Equals(x.Desc, enumDescription, comparison))
            .Select(x => x.Value)
            .FirstOrDefault();

    // This method is adapted from Enum.ToUInt64 (an internal method):
    // https://github.com/dotnet/runtime/blob/bd6cbe3642f51d70839912a6a666e5de747ad581/src/libraries/System.Private.CoreLib/src/System/Enum.cs#L240-L260
    internal static ulong ConvertToUInt64<TEnum>(TEnum enumValue, TypeCode? typeCode = null) where TEnum : Enum
    {
        object value = enumValue;
        typeCode ??= Type.GetTypeCode(typeof(TEnum));

        return typeCode switch
        {
            TypeCode.Int32 => (ulong)(int)value,
            TypeCode.UInt32 => (uint)value,
            TypeCode.UInt64 => (ulong)value,
            TypeCode.Int64 => (ulong)(long)value,
            TypeCode.SByte => (ulong)(sbyte)value,
            TypeCode.Byte => (byte)value,
            TypeCode.Int16 => (ulong)(short)value,
            TypeCode.UInt16 => (ushort)value,
            _ => throw new InvalidOperationException(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TEnum JsonReadEnumNumber<TEnum>(ref Utf8JsonReader reader, TypeCode? typeCode = null)
        where TEnum : struct, Enum =>
        (typeCode ?? Type.GetTypeCode(typeof(TEnum))) switch
        {
            // Switch cases ordered by expected frequency
            TypeCode.Int32 when reader.TryGetInt32(out int int32) => Unsafe.As<int, TEnum>(ref int32),
            TypeCode.UInt32 when reader.TryGetUInt32(out uint uint32) => Unsafe.As<uint, TEnum>(ref uint32),
            TypeCode.UInt64 when reader.TryGetUInt64(out ulong uint64) => Unsafe.As<ulong, TEnum>(ref uint64),
            TypeCode.Int64 when reader.TryGetInt64(out long int64) => Unsafe.As<long, TEnum>(ref int64),
            TypeCode.SByte when reader.TryGetSByte(out sbyte byte8) => Unsafe.As<sbyte, TEnum>(ref byte8),
            TypeCode.Byte when reader.TryGetByte(out byte ubyte8) => Unsafe.As<byte, TEnum>(ref ubyte8),
            TypeCode.Int16 when reader.TryGetInt16(out short int16) => Unsafe.As<short, TEnum>(ref int16),
            TypeCode.UInt16 when reader.TryGetUInt16(out ushort uint16) => Unsafe.As<ushort, TEnum>(ref uint16),
            _ => throw new JsonException(),
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryConvertFromString<TEnum>(string? str, TypeCode? typeCode, out TEnum value)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            value = default;
            return false;
        }

        switch (typeCode ?? Type.GetTypeCode(typeof(TEnum)))
        {
            // Switch cases ordered by expected frequency
            case TypeCode.Int32 when int.TryParse(str, out int int32):
                value = Unsafe.As<int, TEnum>(ref int32);
                return true;
            case TypeCode.UInt32 when uint.TryParse(str, out uint uint32):
                value = Unsafe.As<uint, TEnum>(ref uint32);
                return true;
            case TypeCode.UInt64 when ulong.TryParse(str, out ulong uint64):
                value = Unsafe.As<ulong, TEnum>(ref uint64);
                return true;
            case TypeCode.Int64 when long.TryParse(str, out long int64):
                value = Unsafe.As<long, TEnum>(ref int64);
                return true;
            case TypeCode.SByte when sbyte.TryParse(str, out sbyte byte8):
                value = Unsafe.As<sbyte, TEnum>(ref byte8);
                return true;
            case TypeCode.Byte when byte.TryParse(str, out byte ubyte8):
                value = Unsafe.As<byte, TEnum>(ref ubyte8);
                return true;
            case TypeCode.Int16 when short.TryParse(str, out short int16):
                value = Unsafe.As<short, TEnum>(ref int16);
                return true;
            case TypeCode.UInt16 when ushort.TryParse(str, out ushort uint16):
                value = Unsafe.As<ushort, TEnum>(ref uint16);
                return true;
            default:
                value = default;
                return false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static TEnum TryConvertFromUInt64<TEnum>(ulong longValue, TypeCode? typeCode = null)
        where TEnum : struct, Enum
    {
        switch (typeCode ?? Type.GetTypeCode(typeof(TEnum)))
        {
            // Switch cases ordered by expected frequency
            case TypeCode.Int32:
                var int32 = (int)longValue;
                return Unsafe.As<int, TEnum>(ref int32);
            case TypeCode.UInt32:
                var uint32 = (uint)longValue;
                return Unsafe.As<uint, TEnum>(ref uint32);
            case TypeCode.UInt64:
                var uint64 = longValue;
                return Unsafe.As<ulong, TEnum>(ref uint64);
            case TypeCode.Int64:
                var int64 = (long)longValue;
                return Unsafe.As<long, TEnum>(ref int64);
            case TypeCode.SByte:
                var byte8 = (sbyte)longValue;
                return Unsafe.As<sbyte, TEnum>(ref byte8);
            case TypeCode.Byte:
                var ubyte8 = (byte)longValue;
                return Unsafe.As<byte, TEnum>(ref ubyte8);
            case TypeCode.Int16:
                var int16 = (short)longValue;
                return Unsafe.As<short, TEnum>(ref int16);
            case TypeCode.UInt16:
                var uint16 = (ushort)longValue;
                return Unsafe.As<ushort, TEnum>(ref uint16);
            default:
                return default;
        }
    }

    internal static string? GetFlagListString<TEnum>(TEnum value, JsonNamingPolicy? policy, string separator,
        bool quote = false)
        where TEnum : struct, Enum
    {
        StringBuilder result = new();
        ulong flag = 1;
        var enums = Enum.GetValues<TEnum>();
        for (var index = 0; index < enums.Length; index++)
        {
            var enumValue = enums[index];
            var bits = ConvertToUInt64(enumValue);
            while (flag < bits) flag <<= 1;

            if (flag == bits && value.HasFlag(enumValue))
            {
                if (result.Length > 0)
                    result.Append(separator);

                if (quote) result.Append('"');
                result.Append(policy.ConvertString(Convert.ToString(bits)));
                if (quote) result.Append('"');
            }
        }

        return result.Length is 0 ? null : result.ToString();
    }
}
