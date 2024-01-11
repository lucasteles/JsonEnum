using System.Runtime.Serialization;

namespace JsonEnum.Tests;

public enum EnumForString
{
    Value1,
    Value2,
}

[Flags]
public enum EnumFlagsForString
{
    Value1 = 2,
    Value2 = 4,
    Value3 = 8,
}

public enum EnumForDescription
{
    [Description("First value")] Value1,
    [Description("Second value")] Value2,
}

[Flags]
public enum EnumFlagsForDescription
{
    [Description("First value")] Value1 = 2,
    [Description("Second value")] Value2 = 4,
    [Description("Third value")] Value3 = 8,
}

public enum EnumForMemberValue
{
    [EnumMember(Value = "First value")] Value1,
    [EnumMember(Value = "Second value")] Value2,
}

[Flags]
public enum EnumFlagsForMemberValue
{
    [EnumMember(Value = "First value")] Value1 = 2,
    [EnumMember(Value = "Second value")] Value2 = 4,
    [EnumMember(Value = "Third value")] Value3 = 8,
}
