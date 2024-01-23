using MySql.Data.Types;

namespace OrderService.Model.DTOs;

public class OrderCreate
{
    public int OrderId { get; set; }
    public int ClientId { get; set; }
    public DateTime OrderDate { get; set; }
}