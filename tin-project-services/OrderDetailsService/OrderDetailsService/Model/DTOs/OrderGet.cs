namespace OrderDetailsService.Model.DTOs;

public class OrderGet
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    
    public int UserId { get; set; }
    public UserGet User { get; set; } = null!;
    
}