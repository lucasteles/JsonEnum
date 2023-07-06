using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JsonEnum.Swagger;

/// <summary>
/// Show schema for JsonEnum
/// </summary>
public class EnumJsonSchemaFilter : ISchemaFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) return;

        var converter = context.Type
            .GetCustomAttributes<JsonConverterAttribute>(true)
            .Select(x => x.ConverterType)
            .Where(x => x is not null).Cast<Type>()
            .LastOrDefault(x => x.Name.Contains("Enum") && x.Name.Contains("Json"));

        if (converter is null) return;

        List<IOpenApiAny> items = new();
        StringBuilder descriptions = new();
        foreach (var value in Enum.GetValues(context.Type))
        {
            if (value is null) continue;
            var openApiValue = GetEnumValue(context.Type, converter, value, out var newValue);
            if (openApiValue is null) return;
            items.Add(openApiValue);
            if (!string.IsNullOrWhiteSpace(newValue))
                descriptions.Append($"<li><i>{newValue}</i> - {value.ToString()?.Trim()}</li>");
        }

        schema.Enum.Clear();
        foreach (var item in items) schema.Enum.Add(item);

        if (descriptions.Length > 0)
            schema.Description +=
                $"""
                 <p>Members:</p>
                 <ul>{descriptions}</ul>
                """;
    }

    static IOpenApiAny? GetEnumValue(Type type, Type converter,
        object value, out string? newValue)
    {
        var enumName = type.GetEnumName(value);
        newValue = null;

        if (converter == typeof(JsonEnumNumericConverter))
        {
            var underType = Enum.GetUnderlyingType(type);
            if (underType == typeof(long))
            {
                var longValue = (long)value;
                newValue = longValue.ToString();
                return new OpenApiLong(longValue);
            }

            var intValue = (int)value;
            newValue = intValue.ToString();
            return new OpenApiLong(intValue);
        }

        if (converter == typeof(JsonEnumNumericStringConverter))
        {
            var numberValue = ((long)value).ToString();
            newValue = $"\"{numberValue}\"";
            return new OpenApiString(numberValue);
        }

        if (converter == typeof(JsonStringEnumConverter))
            return new OpenApiString(enumName);

        if (converter == typeof(JsonEnumDescriptionConverter))
            return new OpenApiString(EnumHelpers.GetDescription(value));

        if (converter == typeof(JsonEnumMemberValueConverter))
            return new OpenApiString(EnumHelpers.GetEnumMemberValue(value));

        return null;
    }
}
