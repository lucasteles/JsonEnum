using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonEnum.Swagger;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable S125
#pragma warning disable S3903
#pragma warning disable S2344

// using JsonEnum;
// var flagged = FlagsEnum.Foo | FlagsEnum.Bar;
// Console.WriteLine(flagged.ToString());
// Console.WriteLine(flagged.GetEnumMemberValue());
// Console.WriteLine(flagged.GetDescription());
// Console.WriteLine(JsonSerializer.Deserialize<FlagsEnum>(@"""THE_FOO, Bar""").ToString());
// Console.WriteLine(JsonSerializer.SerializeToElement(NumStrEnum.Bar | NumStrEnum.Foo).GetString());
// Console.WriteLine(JsonSerializer.Deserialize<NumStrEnum>(@"""2, 4""").ToString());
// return;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt =>
    {
        opt.SerializerOptions.Converters.Add(new JsonEnumStringConverter());
        opt.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    })
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(opt =>
    {
        opt.SchemaFilter<JsonEnumSchemaFilter>();
    });

var app = builder.Build();
app.UseSwagger().UseSwaggerUI();

app.MapPost("/api", ([FromBody] RequestData data) => data);
app.MapGet("/api/foo", () =>
    new RequestData(DescriptionEnum.Foo, MemberEnum.Foo, NumStrEnum.Foo, IntEnum.Foo, StrEnum.Foo));
app.MapGet("/api/bar", () =>
    new RequestData(DescriptionEnum.Bar, MemberEnum.Bar, NumStrEnum.Bar, IntEnum.Bar, StrEnum.Bar));

app.Run();

record RequestData(
    DescriptionEnum Description,
    MemberEnum Member,
    NumStrEnum NumStr,
    IntEnum Int,
    StrEnum Str
);

[JsonEnumDescription]
enum DescriptionEnum
{
    [Description("foo_desc")]
    Foo = 1,
    Bar = 2,
}


[JsonEnumMemberValue]
enum MemberEnum
{
    [EnumMember(Value = "foo_member")]
    Foo = 1,
    Bar = 2,
}


[Flags, JsonEnumNumericString]
enum NumStrEnum
{
    None = 0,
    Foo = 2,
    Bar = 4,
}

[JsonEnumNumeric]
enum IntEnum
{
    Foo = 1,
    Bar = 2,
}

[JsonEnumString]
enum StrEnum
{
    Foo = 1,
    Bar = 2,
}

[Flags, JsonEnumDescription]
enum FlagsEnum
{
    None = 0,

    [Description("THE_FOO")]
    Foo = 1 << 1,

    [EnumMember(Value = "THE_BAR")]
    Bar = 1 << 2,
    Baz = 1 << 3,
}
