using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace

namespace JsonEnum.Tests.NumericJson;

using static JsonSerializer;

public class JsonEnumNumericTests : BaseTest
{
    readonly JsonSerializerOptions options = new()
    {
        Converters =
        {
            new JsonNumericEnumConverter(),
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public record TestData(EnumForString Data);

    [TestCase(EnumForString.Value1, 0)]
    [TestCase(EnumForString.Value2, 1)]
    public void ShouldSerialize(EnumForString @enum, int name)
    {
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""data"":{name}}}";

        value.Should().Be(expected);
    }

    [TestCase("0", EnumForString.Value1)]
    [TestCase("1", EnumForString.Value2)]
    [TestCase("\"0\"", EnumForString.Value1)]
    [TestCase("\"1\"", EnumForString.Value2)]
    public void ShouldDeserialize(string name, EnumForString @enum)
    {
        var value = Deserialize<TestData>($@"{{""data"":{name}}}", options)!.Data;

        value.Should().Be(@enum);
    }
}

public class JsonEnumNumericPropertyAttributeTests : BaseTest
{
    readonly JsonSerializerOptions options = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(),
        }, // should be override
    };

    public record TestData([property: JsonEnumNumeric] EnumForString Data);

    [TestCase(EnumForString.Value1, 0)]
    [TestCase(EnumForString.Value2, 1)]
    public void ShouldSerialize(EnumForString @enum, int name)
    {
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""Data"":{name}}}";

        value.Should().Be(expected);
    }

    [TestCase("0", EnumForString.Value1)]
    [TestCase("1", EnumForString.Value2)]
    [TestCase("\"0\"", EnumForString.Value1)]
    [TestCase("\"1\"", EnumForString.Value2)]
    public void ShouldDeserialize(string name, EnumForString @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"":{name}}}", options);

        value!.Data.Should().Be(@enum);
    }
}

public class JsonEnumNumericAttributeTests : BaseTest
{
    [JsonConverter(typeof(JsonStringEnumConverter))] // shoud be override
    public enum TestEnumType
    {
        Value1,
        Value2,
    }

    public record TestData(TestEnumType Data);

    readonly JsonSerializerOptions options = new()
    {
        Converters =
        {
            new JsonNumericEnumConverter(),
        },
    };

    [TestCase(TestEnumType.Value1, 0)]
    [TestCase(TestEnumType.Value2, 1)]
    public void ShouldSerialize(TestEnumType @enum, int name)
    {
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""Data"":{name}}}";

        value.Should().Be(expected);
    }

    [TestCase("0", TestEnumType.Value1)]
    [TestCase("1", TestEnumType.Value2)]
    [TestCase("\"0\"", TestEnumType.Value1)]
    [TestCase("\"1\"", TestEnumType.Value2)]
    public void ShouldDeserialize(string name, TestEnumType @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"":{name}}}", options);

        value!.Data.Should().Be(@enum);
    }
}
