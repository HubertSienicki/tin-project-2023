namespace OrderDetailsService.Model.DTOs;

public class OrderDetailsGet
{
    public int Quantity { get; set; }
    public string AdditionalColumn { get; set; } = default!;
    public int OrderId { get; set; }
    public int ProductId { get; set; }
}