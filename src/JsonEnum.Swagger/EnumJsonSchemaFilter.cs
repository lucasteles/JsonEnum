using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JsonEnum.Swagger;

/// <summary>
/// Show schema for JsonEnum
/// </summary>
public class JsonEnumSchemaFilter : ISchemaFilter
{
    readonly JsonSerializerOptions? jsonOptions;

    /// <summary>
    /// Crate new json enum schema
    /// </summary>
    public JsonEnumSchemaFilter(IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>? options = null) =>
        jsonOptions = options?.Value.SerializerOptions;

    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) return;

        var converter = context.Type
            .GetCustomAttributes<JsonConverterAttribute>(true)
            .Select(x => x.ConverterType)
            .Where(x => x is not null).Cast<Type>()
            .LastOrDefault();

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
            schema.Description += $"\n<p>Members:</p>\n<ul>{descriptions}</ul>\n";
    }

    JsonElement? SerializeValue(object value, Type converterType)
    {
        if (Activator.CreateInstance(converterType) is not JsonConverter converter)
            return null;

        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = jsonOptions?.PropertyNamingPolicy,
            DictionaryKeyPolicy = jsonOptions?.DictionaryKeyPolicy,
            Converters =
            {
                converter,
            },
        };

        return JsonSerializer.SerializeToElement(value, options);
    }

    IOpenApiAny? GetEnumValue(
        Type type, Type converter,
        object value, out string? newValue
    )
    {
        newValue = null;

        if (SerializeValue(value, converter) is not { } jsonValue)
            return new OpenApiString(type.GetEnumName(value));

        var typeCode = Type.GetTypeCode(type);

        return jsonValue.ValueKind switch
        {
            JsonValueKind.String => new OpenApiString(jsonValue.GetString()),
            JsonValueKind.Number when typeCode is TypeCode.Int64 or TypeCode.UInt32 or TypeCode.UInt64 =>
                new OpenApiLong(jsonValue.GetInt64()),
            JsonValueKind.Number => new OpenApiInteger(jsonValue.GetInt32()),
            _ => null,
        };
    }
}
