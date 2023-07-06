namespace JsonEnum.Tests;

public class EnumHelpersTests : BaseTest
{
    [TestCase(EnumForDescription.Value1, "First value")]
    [TestCase(EnumForDescription.Value2, "Second value")]
    public void ShouldGetDescription(EnumForDescription @enum, string expected) =>
        @enum.GetDescription().Should().Be(expected);

    [TestCase(EnumForDescription.Value1, "First value")]
    [TestCase(EnumForDescription.Value2, "Second value")]
    public void ShouldGetDescription(object @enum, string expected) =>
        EnumHelpers.GetDescription(@enum).Should().Be(expected);

    [TestCase("First value", EnumForDescription.Value1)]
    [TestCase("Second value", EnumForDescription.Value2)]
    public void ShouldGetDescription(string description, EnumForDescription expected) =>
        EnumHelpers.GetEnumFromDescription<EnumForDescription>(description).Should().Be(expected);

    [TestCase(EnumForMemberValue.Value1, "First value")]
    [TestCase(EnumForMemberValue.Value2, "Second value")]
    public void ShouldGetDescription(EnumForMemberValue @enum, string expected) =>
        @enum.GetEnumMemberValue().Should().Be(expected);

    [TestCase("First value", EnumForMemberValue.Value1)]
    [TestCase("Second value", EnumForMemberValue.Value2)]
    public void ShouldGetDescription(string name, EnumForMemberValue expected) =>
        EnumHelpers.GetEnumFromEnumMemberValue<EnumForMemberValue>(name).Should().Be(expected);
}
