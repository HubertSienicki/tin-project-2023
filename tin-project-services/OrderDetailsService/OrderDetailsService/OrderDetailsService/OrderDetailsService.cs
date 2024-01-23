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

    public Tuple<bool, string> ValidateJsonSchema(object value, string schemaPrefix)
    {
        var jsonSchema = ReadSchema(schemaPrefix);

        // schema should be camelCase
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var json = JsonConvert.SerializeObject(value, settings);
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
        var schemaFilePath = Path.Combine("/app", "Model/DTOs/JsonSchemas", $"{schemaPrefix}.json");
        if (!File.Exists(schemaFilePath))
        {
            Console.WriteLine($"Schema file not found: {schemaFilePath}");
            // Handle file not found appropriately
        }

        var reader = new StreamReader(schemaFilePath);
        var sb = new StringBuilder();
        var line = "";

        while ((line = reader.ReadLine()) != null) sb.Append(line);
        reader.Close();
        return sb.ToString();
    }


}