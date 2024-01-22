namespace OrderDetailsService.Model.DTOs;

public class OrderDetailsGet
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    public int OrderId { get; set; }
    public OrderGet Order { get; set; } = default!;
    public int ProductId { get; set; }
    public ProductGet Product { get; set; } = default!;
}