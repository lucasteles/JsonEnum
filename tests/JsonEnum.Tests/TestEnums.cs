using System.Runtime.Serialization;

namespace JsonEnum.Tests;

public enum EnumForString
{
    Value1,
    Value2,
}

public enum EnumForDescription
{
    [Description("First value")] Value1,
    [Description("Second value")] Value2,
}

public enum EnumForMemberValue
{
    [EnumMember(Value = "First value")] Value1,
    [EnumMember(Value = "Second value")] Value2,
}
