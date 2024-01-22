namespace OrderDetailsService.Model;

public class Order
{
    // Primary key
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    
    // Foreign key
    public int UserId { get; set; }
    public User? User { get; set; }
}