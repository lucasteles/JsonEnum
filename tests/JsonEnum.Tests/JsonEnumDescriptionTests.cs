using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable CheckNamespace

namespace JsonEnum.Tests.Description;

using static JsonSerializer;

public class JsonEnumDescriptionTests : BaseTest
{
    readonly JsonSerializerOptions options = new()
    {
        Converters =
        {
            new JsonEnumDescriptionConverter()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public record TestData(EnumForDescription Data);

    [TestCase(EnumForDescription.Value1, "First value")]
    [TestCase(EnumForDescription.Value2, "Second value")]
    public void ShouldSerialize(EnumForDescription @enum, string description)
    {
        var value = Deserialize<TestData>($@"{{""data"": ""{description}""}}", options)!.Data;

        value.Should().Be(@enum);
    }

    [TestCase("First value", EnumForDescription.Value1)]
    [TestCase("Second value", EnumForDescription.Value2)]
    public void ShouldDeserialize(string description, EnumForDescription @enum)
    {
        var value = Serialize(new TestData(@enum), options);
        var expected = $@"{{""data"":""{description}""}}";

        value.Should().Be(expected);
    }
}

public class JsonEnumDescriptionPropertyAttributeTests : BaseTest
{
    public record TestData([property: JsonEnumDescription] EnumForDescription Data);

    [TestCase(EnumForDescription.Value1, "First value")]
    [TestCase(EnumForDescription.Value2, "Second value")]
    public void ShouldSerialize(EnumForDescription @enum, string description)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{description}""}}");

        value!.Data.Should().Be(@enum);
    }

    [TestCase("First value", EnumForDescription.Value1)]
    [TestCase("Second value", EnumForDescription.Value2)]
    public void ShouldDeserialize(string description, EnumForDescription @enum)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{description}""}}";

        value.Should().Be(expected);
    }
}

public class JsonEnumDescriptionAttributeTests : BaseTest
{
    [JsonEnumDescription]
    public enum TestEnumType
    {
        [Description("first_value")] Value1,
        [Description("second_value")] Value2,
    }

    public record TestData(TestEnumType Data);

    [TestCase(TestEnumType.Value1, "first_value")]
    [TestCase(TestEnumType.Value2, "second_value")]
    public void ShouldSerialize(TestEnumType @enum, string description)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{description}""}}");

        value!.Data.Should().Be(@enum);
    }

    [TestCase("first_value", TestEnumType.Value1)]
    [TestCase("second_value", TestEnumType.Value2)]
    public void ShouldDeserialize(string description, TestEnumType @enum)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{description}""}}";

        value.Should().Be(expected);
    }
}

public class JsonEnumDescriptionFlagsTests : BaseTest
{
    public record TestData([property: JsonEnumDescription] EnumFlagsForDescription Data);

    [TestCase(EnumFlagsForDescription.Value1, "Desc1")]
    [TestCase(EnumFlagsForDescription.Value2, "Desc2")]
    [TestCase(EnumFlagsForDescription.Value3, "Desc3")]
    [TestCase(EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value2, "Desc1, Desc2")]
    [TestCase(EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value3, "Desc1, Desc3")]
    [TestCase(EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value2 | EnumFlagsForDescription.Value3,
        "Desc1, Desc2, Desc3")]
    public void ShouldSerialize(EnumFlagsForDescription @enum, string name)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Desc1", EnumFlagsForDescription.Value1)]
    [TestCase("Desc2", EnumFlagsForDescription.Value2)]
    [TestCase("Desc3", EnumFlagsForDescription.Value3)]
    [TestCase("Desc1, Desc2", EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value2)]
    [TestCase("Desc1, Desc3", EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value3)]
    [TestCase("Desc1, Desc2, Desc3",
        EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value2 | EnumFlagsForDescription.Value3)]
    public void ShouldDeserialize(string name, EnumFlagsForDescription @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }
}

public class JsonEnumDescriptionFlagsSepTests : BaseTest
{
    public record TestData(
        [property: JsonEnumDescription(FlagsValueSeparator = "|")]
        EnumFlagsForDescription Data
    );

    [TestCase(EnumFlagsForDescription.Value1, "Desc1")]
    [TestCase(EnumFlagsForDescription.Value2, "Desc2")]
    [TestCase(EnumFlagsForDescription.Value3, "Desc3")]
    [TestCase(EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value2, "Desc1|Desc2")]
    [TestCase(EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value3, "Desc1|Desc3")]
    [TestCase(EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value2 | EnumFlagsForDescription.Value3,
        "Desc1|Desc2|Desc3")]
    public void ShouldSerialize(EnumFlagsForDescription @enum, string name)
    {
        var value = Serialize(new TestData(@enum));
        var expected = $@"{{""Data"":""{name}""}}";

        value.Should().Be(expected);
    }

    [TestCase("Desc1", EnumFlagsForDescription.Value1)]
    [TestCase("Desc2", EnumFlagsForDescription.Value2)]
    [TestCase("Desc3", EnumFlagsForDescription.Value3)]
    [TestCase("Desc1|Desc2", EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value2)]
    [TestCase("Desc1|Desc3", EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value3)]
    [TestCase("Desc1|Desc2|Desc3",
        EnumFlagsForDescription.Value1 | EnumFlagsForDescription.Value2 | EnumFlagsForDescription.Value3)]
    public void ShouldDeserialize(string name, EnumFlagsForDescription @enum)
    {
        var value = Deserialize<TestData>($@"{{""Data"": ""{name}""}}");

        value!.Data.Should().Be(@enum);
    }
}
