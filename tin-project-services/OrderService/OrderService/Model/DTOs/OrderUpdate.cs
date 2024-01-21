namespace OrderService.Model.DTOs;

public class OrderUpdate
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public UserGet User { get; set; }
}