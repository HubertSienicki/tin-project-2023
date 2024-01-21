namespace OrderService.Model;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    
    // foreign
    public int UserId { get; set; }
    public User User { get; set; }
}