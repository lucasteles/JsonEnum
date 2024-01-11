using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace

namespace JsonEnum.Tests.StringName;

using static JsonSerializer;

[TestFixture(typeof(JsonEnumStringConverter))]
[TestFixture(typeof(JsonEnumStringConverter<EnumForString>))]
public class JsonEnumStringTests : BaseTest
{
    readonly JsonSerializerOptions options;

    public JsonEnumStringTests(Type converter) =>
        options = new()
        {
            Converters =
            {
                (JsonConverter)Activator.CreateInstance(converter)!,
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

    public record TestData(EnumForString Data);

    [TestCase(EnumForString.Value1, "Value1")]
    [TestCase(EnumForString.Value2, "Value2")]
    public void ShouldSerialize(EnumForString @enum, string name)
    {
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Value1", EnumForString.Value1)]
    [TestCase("Value2", EnumForString.Value2)]
    public void ShouldDeserialize(string name, EnumForString @enum)
    {
        var value = Deserialize<TestData>($@"{{""data"": ""{name}""}}", options)!.Data;

        value.Should().Be(@enum);
    }
}

public class JsonEnumStringPropertyAttributeTests : BaseTest
{
    public record TestData([property: JsonEnumString] EnumForString Data);

    [TestCase(EnumForString.Value1, "Value1")]
    [TestCase(EnumForString.Value2, "Value2")]
    public void ShouldSerialize(EnumForString @enum, string name)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Value1", EnumForString.Value1)]
    [TestCase("Value2", EnumForString.Value2)]
    public void ShouldDeserialize(string name, EnumForString @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }
}

public class JsonEnumStringAttributeTests : BaseTest
{
    readonly JsonSerializerOptions options = new()
    {
        Converters =
        {
            new JsonEnumNumericConverter(),
        }
    };

    [JsonEnumString]
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
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}", options);

        value!.Data.Should().Be(@enum);
    }

    [TestCase("Value1", TestEnumType.Value1)]
    [TestCase("Value2", TestEnumType.Value2)]
    public void ShouldDeserialize(string name, TestEnumType @enum)
    {
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }
}
