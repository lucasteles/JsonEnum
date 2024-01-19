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
        },
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
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Value1", TestEnumType.Value1)]
    [TestCase("Value2", TestEnumType.Value2)]
    public void ShouldDeserialize(string name, TestEnumType @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}", options);

        value!.Data.Should().Be(@enum);
    }
}

public class JsonEnumStringFlagsTests : BaseTest
{
    public record TestData([property: JsonEnumString] EnumFlagsForString Data);

    [TestCase(EnumFlagsForString.Value1, "Value1")]
    [TestCase(EnumFlagsForString.Value2, "Value2")]
    [TestCase(EnumFlagsForString.Value3, "Value3")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value2, "Value1, Value2")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value3, "Value1, Value3")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value2 | EnumFlagsForString.Value3,
        "Value1, Value2, Value3")]
    public void ShouldSerialize(EnumFlagsForString @enum, string name)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Value1", EnumFlagsForString.Value1)]
    [TestCase("Value2", EnumFlagsForString.Value2)]
    [TestCase("Value3", EnumFlagsForString.Value3)]
    [TestCase("Value1, Value2", EnumFlagsForString.Value1 | EnumFlagsForString.Value2)]
    [TestCase("Value1, Value3", EnumFlagsForString.Value1 | EnumFlagsForString.Value3)]
    [TestCase("Value1, Value2, Value3",
        EnumFlagsForString.Value1 | EnumFlagsForString.Value2 | EnumFlagsForString.Value3)]
    public void ShouldDeserialize(string name, EnumFlagsForString @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }
}

public class JsonEnumStringFlagsSepTests : BaseTest
{
    public record TestData(
        [property: JsonEnumString(FlagsValueSeparator = "|")]
        EnumFlagsForString Data);

    [TestCase(EnumFlagsForString.Value1, "Value1")]
    [TestCase(EnumFlagsForString.Value2, "Value2")]
    [TestCase(EnumFlagsForString.Value3, "Value3")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value2, "Value1|Value2")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value3, "Value1|Value3")]
    [TestCase(EnumFlagsForString.Value1 | EnumFlagsForString.Value2 | EnumFlagsForString.Value3,
        "Value1|Value2|Value3")]
    public void ShouldSerialize(EnumFlagsForString @enum, string name)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Value1", EnumFlagsForString.Value1)]
    [TestCase("Value2", EnumFlagsForString.Value2)]
    [TestCase("Value3", EnumFlagsForString.Value3)]
    [TestCase("Value1|Value2", EnumFlagsForString.Value1 | EnumFlagsForString.Value2)]
    [TestCase("Value1|Value3", EnumFlagsForString.Value1 | EnumFlagsForString.Value3)]
    [TestCase("Value1|Value2|Value3",
        EnumFlagsForString.Value1 | EnumFlagsForString.Value2 | EnumFlagsForString.Value3)]
    public void ShouldDeserialize(string name, EnumFlagsForString @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }
}

public class JsonEnumStringNamingCaseTests : BaseTest
{
    public JsonSerializerOptions GetOptions(JsonEnumNamingPolicy? policy) =>
        new()
        {
            Converters =
            {
                new JsonEnumStringConverter(policy?.ToJsonNamingPolicy()),
            },
        };

    public enum TestEnumCases
    {
        TheValue1,
        TheValue2,
    }

    public record TestData(TestEnumCases Data);

    [TestCase(TestEnumCases.TheValue1, "TheValue1", null)]
    [TestCase(TestEnumCases.TheValue2, "TheValue2", null)]
    [TestCase(TestEnumCases.TheValue1, "theValue1", JsonEnumNamingPolicy.CamelCase)]
    [TestCase(TestEnumCases.TheValue2, "theValue2", JsonEnumNamingPolicy.CamelCase)]
#if NET8_0_OR_GREATER
    [TestCase(TestEnumCases.TheValue1, "the-value1", JsonEnumNamingPolicy.KebabCaseLower)]
    [TestCase(TestEnumCases.TheValue2, "the-value2", JsonEnumNamingPolicy.KebabCaseLower)]
    [TestCase(TestEnumCases.TheValue1, "THE-VALUE1", JsonEnumNamingPolicy.KebabCaseUpper)]
    [TestCase(TestEnumCases.TheValue2, "THE-VALUE2", JsonEnumNamingPolicy.KebabCaseUpper)]
    [TestCase(TestEnumCases.TheValue1, "the_value1", JsonEnumNamingPolicy.SnakeCaseLower)]
    [TestCase(TestEnumCases.TheValue2, "the_value2", JsonEnumNamingPolicy.SnakeCaseLower)]
    [TestCase(TestEnumCases.TheValue1, "THE_VALUE1", JsonEnumNamingPolicy.SnakeCaseUpper)]
    [TestCase(TestEnumCases.TheValue2, "THE_VALUE2", JsonEnumNamingPolicy.SnakeCaseUpper)]
#endif
    public void ShouldSerialize(TestEnumCases @enum, string name, JsonEnumNamingPolicy? policy)
    {
        var value = Serialize(new TestData(@enum), GetOptions(policy));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("TheValue1", TestEnumCases.TheValue1, null)]
    [TestCase("TheValue2", TestEnumCases.TheValue2, null)]
    [TestCase("theValue1", TestEnumCases.TheValue1, JsonEnumNamingPolicy.CamelCase)]
    [TestCase("theValue2", TestEnumCases.TheValue2, JsonEnumNamingPolicy.CamelCase)]
#if NET8_0_OR_GREATER
    [TestCase("the-value1", TestEnumCases.TheValue1, JsonEnumNamingPolicy.KebabCaseLower)]
    [TestCase("the-value2", TestEnumCases.TheValue2, JsonEnumNamingPolicy.KebabCaseLower)]
    [TestCase("THE-VALUE1", TestEnumCases.TheValue1, JsonEnumNamingPolicy.KebabCaseUpper)]
    [TestCase("THE-VALUE2", TestEnumCases.TheValue2, JsonEnumNamingPolicy.KebabCaseUpper)]
    [TestCase("the_value1", TestEnumCases.TheValue1, JsonEnumNamingPolicy.SnakeCaseLower)]
    [TestCase("the_value2", TestEnumCases.TheValue2, JsonEnumNamingPolicy.SnakeCaseLower)]
    [TestCase("THE_VALUE1", TestEnumCases.TheValue1, JsonEnumNamingPolicy.SnakeCaseUpper)]
    [TestCase("THE_VALUE2", TestEnumCases.TheValue2, JsonEnumNamingPolicy.SnakeCaseUpper)]
#endif
#if NET8_OR_GREATER
#endif
    public void ShouldDeserialize(string name, TestEnumCases @enum, JsonEnumNamingPolicy? policy)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}", GetOptions(policy))!.Data;

        value.Should().Be(@enum);
    }
}
