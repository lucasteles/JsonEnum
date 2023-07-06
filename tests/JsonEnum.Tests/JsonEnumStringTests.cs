using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace

namespace JsonEnum.Tests.StringName;

using static JsonSerializer;

public class JsonEnumStringTests : BaseTest
{
    readonly JsonSerializerOptions options = new()
    {
        Converters = { new JsonStringEnumConverter() }, // <- default converter
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public record TestData(EnumForString Data);

    [TestCase(EnumForString.Value1, "Value1")]
    [TestCase(EnumForString.Value2, "Value2")]
    public void ShouldSerialize(EnumForString @enum, string name)
    {
        var value = Deserialize<TestData>($@"{{""data"": ""{name}""}}", options)!.Data;

        value.Should().Be(@enum);
    }

    [TestCase("Value1", EnumForString.Value1)]
    [TestCase("Value2", EnumForString.Value2)]
    public void ShouldDeserialize(string name, EnumForString @enum)
    {
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""data"":""{name}""}}";

        value.Should().Be(expected);
    }
}

public class JsonEnumStringPropertyAttributeTests : BaseTest
{
    public record TestData([property: JsonEnumString] EnumForString Data);

    [TestCase(EnumForString.Value1, "Value1")]
    [TestCase(EnumForString.Value2, "Value2")]
    public void ShouldSerialize(EnumForString @enum, string name)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }

    [TestCase("Value1", EnumForString.Value1)]
    [TestCase("Value2", EnumForString.Value2)]
    public void ShouldDeserialize(string name, EnumForString @enum)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }
}

public class JsonEnumStringAttributeTests : BaseTest
{
    [JsonEnumDescription]
    public enum TestEnumType
    {
        Value1,
        Value2,
    }

    public record TestData(TestEnumType Data);

    [TestCase(TestEnumType.Value1, "Value1")]
    [TestCase(TestEnumType.Value2, "Value2")]
    public void ShouldSerialize(TestEnumType @enum, string name)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }

    [TestCase("Value1", TestEnumType.Value1)]
    [TestCase("Value2", TestEnumType.Value2)]
    public void ShouldDeserialize(string name, TestEnumType @enum)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }
}
