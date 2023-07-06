[![CI](https://github.com/lucasteles/JsonEnum/actions/workflows/ci.yml/badge.svg)](https://github.com/lucasteles/JsonEnum/actions/workflows/ci.yml)
[![Nuget](https://img.shields.io/nuget/v/JsonEnum.svg?style=flat)](https://www.nuget.org/packages/JsonEnum)
![](https://raw.githubusercontent.com/lucasteles/JsonEnum/badges/badge_linecoverage.svg)
![](https://raw.githubusercontent.com/lucasteles/JsonEnum/badges/badge_branchcoverage.svg)
![](https://raw.githubusercontent.com/lucasteles/JsonEnum/badges/test_report_badge.svg)
![](https://raw.githubusercontent.com/lucasteles/JsonEnum/badges/lines_badge.svg)

![](https://raw.githubusercontent.com/lucasteles/JsonEnum/badges/dotnet_version_badge.svg)
![](https://img.shields.io/badge/Lang-C%23-green)
![https://editorconfig.org/](https://img.shields.io/badge/style-EditorConfig-black)

# JsonEnum

Converters to customize your enum serialization on `System.Text.Json`

## Getting started

[NuGet package](https://www.nuget.org/packages/JsonEnum) available:

```ps
$ dotnet add package JsonEnum
```

## Enum Converters

This library defines the following converters:

- `JsonEnumDescriptionConverter`
- `JsonEnumMemberValueConverter`
- `JsonEnumNumericStringConverter`
- `JsonEnumNumericConverter`

For each converter above there is also an named attribute to set converters on properties or types.

There is also two others converter attributes to force one of the predefined behaviors(`number`| `string`), useful
when you have already set a default converter for enums but need a type to be serialized as number or just the name.
they are:

- `JsonEnumString` - _applies EnumStringJsonConverter_
- `JsonEnumNumeric` - _force to use number value_

### Using [Description]

Enabled adding `JsonEnumDescriptionConverter` to your json options or using the attribute `[JsonEnumDescription]` on
your type.

````csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel;

public enum MyEnum
{
    [Description("first_value")] Value1,
    [Description("second_value")] Value2,
}
public class Foo
{
    [JsonEnumDescription]
    public MyEnum Value { get; init; }
}

var foo = new Foo { Value = MyEnum.Value1 };
````

On the above sample `foo` will be serialized to/from:

```json
{
    "Value": "first_value"
}
```

### EnumMember [Description]

Enabled adding `JsonEnumDescriptionConverter` to your json options or using the attribute `[JsonEnumDescription]` on
your type.

````csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

public enum MyEnum
{
    [EnumMember(Value = "first-value")] Value1,
    [EnumMember(Value = "second-value")] Value2,
}
public class Foo
{
    [JsonEnumMemberValue]
    public MyEnum Value { get; init; }
}

var foo = new Foo { Value = MyEnum.Value1 };
````

On the above sample `foo` will be serialized to/from:

```json
{
    "Value": "first-value"
}
```

### EnumMember [Description]

Enabled adding `JsonEnumDescriptionConverter` to your json options or using the attribute `[JsonEnumDescription]` on
your type.

````csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

public enum MyEnum
{
    [EnumMember(Value = "first-value")] Value1,
    [EnumMember(Value = "second-value")] Value2,
}
public class Foo
{
    [JsonEnumMemberValue]
    public MyEnum Value { get; init; }
}

var foo = new Foo { Value = MyEnum.Value1 };
````

On the above sample `foo` will be serialized to/from:

```json
{
    "Value": "first-value"
}
```

### Numeric string

Serialize enum as a string of the **numeric** value of the enum.

````csharp
using System.Text.Json;
using System.Text.Json.Serialization;

public enum MyEnum
{
    Value1 = 10,
    Value2 = 20,
}
public class Foo
{
    [JsonEnumNumericString]
    public MyEnum Value { get; init; }
}

var foo = new Foo { Value = MyEnum.Value1 };
````

On the above sample `foo` will be serialized to/from:

```json
{
    "Value": "10"
}
```

### Numeric

Usually you will don't need this because is the default behavior of enum serialization in .NET, but can be used when
you already have set a converter on `ASP.NET` or `JsonSerializerOptions.Converters`

````csharp
using System.Text.Json;
using System.Text.Json.Serialization;

public enum MyEnum
{
    Value1 = 10,
    Value2 = 20,
}
public class Foo
{
    [JsonEnumNumeric]
    public MyEnum Value { get; init; }
}

var foo = new Foo { Value = MyEnum.Value1 };

JsonSerializer.Serialize(foo, new JsonSerializerOptions()
{
    Converters = { new JsonStringEnumConverter() }, // will be ignored on Foo type
};
)
````

On the above sample `foo` will be serialized to/from:

```json
{
    "Value": 10
}
```

## Swagger

If you are using `Swashbuckle.AspNetCore` you will notice that those attributes and converters are not respected by
the `swagger` schema. You can use [**this nuget package**](https://www.nuget.org/packages/JsonEnum.Swagger) to fix it:

```ps
$ dotnet add package JsonEnum.Swagger
```

And use the `EnumJsonSchemaFilter` on your swagger configuration:

```csharp
services.AddSwaggerGen(options =>
{
    options.SchemaFilter<EnumJsonSchemaFilter>();
});
```

