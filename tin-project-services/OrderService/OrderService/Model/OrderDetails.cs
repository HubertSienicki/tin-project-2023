namespace OrderService.Model;

public class OrderDetails
{
    public string additionalColumn { get; set; } = null!;
    
    //foreign
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    //navigation
    public Order? Order { get; set; }
    public Product? Product { get; set; }
    
}