namespace OrderService.Model.DTOs;

public class OrderGet
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public Client Client { get; set; }
}