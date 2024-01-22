using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using OrderDetailsService.Model.DTOs;
using OrderDetailsService.OrderDetailsService.Interfaces;

namespace OrderDetailsService.OrderDetailsService;

public class OrderDetailsService : IOrderDetailsService
{
    public Tuple<bool, string> ValidateJsonSchema(OrderDetailsPost orderDetailsPost, string schemaPrefix)
    {
        var jsonSchema = ReadSchema(schemaPrefix);

        // schema should be camelCase
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var json = JsonConvert.SerializeObject(orderDetailsPost, settings);
        Console.WriteLine(json);
        var jsonObject = JObject.Parse(json);
        var schema = JSchema.Parse(jsonSchema);

        var valid = jsonObject.IsValid(schema, out IList<string> errorMessages);

        var sb = new StringBuilder();

        if (valid) return new Tuple<bool, string>(true, "JSON is valid");
        
        sb.Append("JSON is not valid: \n");
        foreach (var error in errorMessages) sb.Append(error + "\n");

        return new Tuple<bool, string>(false, sb.ToString());
    }

    private static string ReadSchema(string schemaPrefix)
    {
        var reader = new StreamReader($"../OrderDetailsService/Model/DTOs/JsonSchemas/{schemaPrefix}.json");
        var sb = new StringBuilder();
        var line = "";

        while ((line = reader.ReadLine()) != null) sb.Append(line);
        reader.Close();
        return sb.ToString();
    }
}