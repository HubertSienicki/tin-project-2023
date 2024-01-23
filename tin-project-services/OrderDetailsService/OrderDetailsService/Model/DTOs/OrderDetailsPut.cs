namespace OrderDetailsService.Model.DTOs;

public class OrderDetailsPut
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string AdditionalColumn { get; set; } = default!;
}