namespace OrderService.Model.DTOs;

public class ProductGet
{
    public int Id { get; set; }
    public string name { get; set; } = null!;
    public decimal price { get; set; }
}