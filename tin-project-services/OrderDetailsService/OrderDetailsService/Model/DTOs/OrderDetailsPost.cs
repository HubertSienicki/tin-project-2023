namespace OrderDetailsService.Model.DTOs;

public class OrderDetailsPost
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string AdditionalColumn { get; set; } = default!;
}