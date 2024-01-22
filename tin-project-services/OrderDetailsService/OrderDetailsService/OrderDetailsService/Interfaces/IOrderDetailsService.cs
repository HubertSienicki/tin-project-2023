using OrderDetailsService.Model.DTOs;

namespace OrderDetailsService.OrderDetailsService.Interfaces;

public interface IOrderDetailsService
{
    public Tuple<bool, string> ValidateJsonSchema(object value, string schemaPrefix);
}