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
            new JsonEnumNumericConverter(),
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
    [JsonEnumNumeric]
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
            new JsonEnumStringConverter(),
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

public class JsonEnumNumericFlagsTests : BaseTest
{
    public record TestData([property: JsonEnumNumeric] EnumFlagsForString Data);

    [TestCase(EnumFlagsForString.Value1, "2")]
    [TestCase(EnumFlagsForString.Value2, "4")]
    [TestCase(EnumFlagsForString.Value3, "8")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value2, "6")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value3, "10")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value2 | EnumFlagsForString.Value3, "14")]
    public void ShouldSerialize(EnumFlagsForString @enum, string name)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":{name}}}";

        value.Should().Be(expected);
    }

    [TestCase("2", EnumFlagsForString.Value1)]
    [TestCase("4", EnumFlagsForString.Value2)]
    [TestCase("8", EnumFlagsForString.Value3)]
    [TestCase("6", EnumFlagsForString.Value1 | EnumFlagsForString.Value2)]
    [TestCase("10", EnumFlagsForString.Value1 | EnumFlagsForString.Value3)]
    [TestCase("14", EnumFlagsForString.Value1 | EnumFlagsForString.Value2 | EnumFlagsForString.Value3)]
    public void ShouldDeserialize(string name, EnumFlagsForString @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": {name}}}");

        value!.Data.Should().Be(@enum);
    }
}
