using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonEnum.Swagger;
using Microsoft.AspNetCore.Mvc;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

#pragma warning disable S3903
#pragma warning disable S2344

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<JsonOptions>(opt =>
    {
        opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(opt =>
    {
        opt.SchemaFilter<JsonEnumSchemaFilter>();
    });

var app = builder.Build();
app.UseSwagger().UseSwaggerUI();

app.MapPost("/api", ([FromBody] RequestData data) => data);

app.Run();

record RequestData(
    DescriptionEnum Description,
    MemberEnum Member,
    NumStrEnum NumStr,
    IntEnum Int
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


[JsonEnumNumericString]
enum NumStrEnum
{
    Foo = 1,
    Bar = 2,
}

[JsonEnumNumeric]
enum IntEnum
{
    Foo = 1,
    Bar = 2,
}
