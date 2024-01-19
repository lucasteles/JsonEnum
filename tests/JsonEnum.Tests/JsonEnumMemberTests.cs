using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace

namespace JsonEnum.Tests.EnumMemberValue;

using static JsonSerializer;

public class JsonEnumEnumMemberTests : BaseTest
{
    readonly JsonSerializerOptions options = new()
    {
        Converters =
        {
            new JsonEnumMemberValueConverter(),
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public record TestData(EnumForMemberValue Data);

    [TestCase(EnumForMemberValue.Value1, "First value")]
    [TestCase(EnumForMemberValue.Value2, "Second value")]
    public void ShouldSerialize(EnumForMemberValue @enum, string name)
    {
        var value = Deserialize<TestData>($@"{{""data"": ""{name}""}}", options)!.Data;

        value.Should().Be(@enum);
    }

    [TestCase("First value", EnumForMemberValue.Value1)]
    [TestCase("Second value", EnumForMemberValue.Value2)]
    public void ShouldDeserialize(string name, EnumForMemberValue @enum)
    {
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""data"":""{name}""}}";

        value.Should().Be(expected);
    }
}

public class JsonEnumDescriptionPropertyAttributeTests : BaseTest
{
    public record TestData([property: JsonEnumMemberValue] EnumForMemberValue Data);

    [TestCase(EnumForMemberValue.Value1, "First value")]
    [TestCase(EnumForMemberValue.Value2, "Second value")]
    public void ShouldSerialize(EnumForMemberValue @enum, string name)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }

    [TestCase("First value", EnumForMemberValue.Value1)]
    [TestCase("Second value", EnumForMemberValue.Value2)]
    public void ShouldDeserialize(string name, EnumForMemberValue @enum)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }
}

public class JsonEnumDescriptionAttributeTests : BaseTest
{
    [JsonEnumMemberValue]
    public enum TestEnumType
    {
        [EnumMember(Value = "first_value")] Value1,
        [EnumMember(Value = "second_value")] Value2,
    }

    public record TestData(TestEnumType Data);

    [TestCase(TestEnumType.Value1, "first_value")]
    [TestCase(TestEnumType.Value2, "second_value")]
    public void ShouldSerialize(TestEnumType @enum, string name)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }

    [TestCase("first_value", TestEnumType.Value1)]
    [TestCase("second_value", TestEnumType.Value2)]
    public void ShouldDeserialize(string name, TestEnumType @enum)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }
}

public class JsonEnumMemberValueFlagsTests : BaseTest
{
    public record TestData([property: JsonEnumMemberValue] EnumFlagsForMemberValue Data);

    [TestCase(EnumFlagsForMemberValue.Value1, "Member1")]
    [TestCase(EnumFlagsForMemberValue.Value2, "Member2")]
    [TestCase(EnumFlagsForMemberValue.Value3, "Member3")]
    [TestCase(EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value2, "Member1, Member2")]
    [TestCase(EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value3, "Member1, Member3")]
    [TestCase(EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value2 | EnumFlagsForMemberValue.Value3,
        "Member1, Member2, Member3")]
    public void ShouldSerialize(EnumFlagsForMemberValue @enum, string name)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Member1", EnumFlagsForMemberValue.Value1)]
    [TestCase("Member2", EnumFlagsForMemberValue.Value2)]
    [TestCase("Member3", EnumFlagsForMemberValue.Value3)]
    [TestCase("Member1, Member2", EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value2)]
    [TestCase("Member1, Member3", EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value3)]
    [TestCase("Member1, Member2, Member3",
        EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value2 | EnumFlagsForMemberValue.Value3)]
    public void ShouldDeserialize(string name, EnumFlagsForMemberValue @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }
}

public class JsonEnumMemberValueFlagsSepTests : BaseTest
{
    public record TestData(
        [property: JsonEnumMemberValue(FlagsValueSeparator = "|")]
        EnumFlagsForMemberValue Data
    );

    [TestCase(EnumFlagsForMemberValue.Value1, "Member1")]
    [TestCase(EnumFlagsForMemberValue.Value2, "Member2")]
    [TestCase(EnumFlagsForMemberValue.Value3, "Member3")]
    [TestCase(EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value2, "Member1|Member2")]
    [TestCase(EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value3, "Member1|Member3")]
    [TestCase(EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value2 | EnumFlagsForMemberValue.Value3,
        "Member1|Member2|Member3")]
    public void ShouldSerialize(EnumFlagsForMemberValue @enum, string name)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Member1", EnumFlagsForMemberValue.Value1)]
    [TestCase("Member2", EnumFlagsForMemberValue.Value2)]
    [TestCase("Member3", EnumFlagsForMemberValue.Value3)]
    [TestCase("Member1|Member2", EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value2)]
    [TestCase("Member1|Member3", EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value3)]
    [TestCase("Member1|Member2|Member3",
        EnumFlagsForMemberValue.Value1 | EnumFlagsForMemberValue.Value2 | EnumFlagsForMemberValue.Value3)]
    public void ShouldDeserialize(string name, EnumFlagsForMemberValue @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }
}
