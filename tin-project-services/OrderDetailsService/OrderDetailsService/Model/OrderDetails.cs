namespace OrderDetailsService.Model;

public class OrderDetails
{
    // Primary key
    public int Quantity { get; set; }
    public string AdditionalColumn { get; set; }
    
    // Foreign key
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}