namespace OrderDetailsService.Model.DTOs;

public class OrderGet
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public Client Client { get; set; }
}