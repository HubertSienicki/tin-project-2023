namespace OrderService.Model.DTOs;

public class OrderDetailsGet
{
    // properties
    public string AdditionalColumn { get; set; } = null!;
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    
    // navigation
    public OrderGet Order { get; set; }
    public List<ProductGet>? Product { get; set; }
    
}