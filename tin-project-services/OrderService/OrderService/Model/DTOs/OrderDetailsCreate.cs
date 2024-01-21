namespace OrderService.Model.DTOs;

public class OrderDetailsCreate
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string AdditionalColumn { get; set; } = null!;
}