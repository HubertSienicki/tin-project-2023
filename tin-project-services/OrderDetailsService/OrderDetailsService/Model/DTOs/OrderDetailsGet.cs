namespace OrderDetailsService.Model.DTOs;

public class OrderDetailsGet
{
    public string AdditionalColumn { get; set; } = null!;
    public int Quantity { get; set; }
    
    // navigation
    public OrderGet Order { get; set; } = null!;
    public List<ProductGet>? Product { get; set; }
}