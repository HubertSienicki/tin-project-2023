namespace OrderService.Model.DTOs;

public class ProductPut
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public decimal price { get; set; }
}