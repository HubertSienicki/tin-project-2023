using MySql.Data.Types;

namespace OrderService.Model.DTOs;

public class OrderCreate
{
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
}