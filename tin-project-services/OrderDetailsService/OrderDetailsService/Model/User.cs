namespace OrderDetailsService.Model;

public class User
{
    // Primary key
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? PasswordSalt { get; set; }
    
    // Foreign key
    public string? RoleId { get; set; }
    public Role? Role { get; set; }
}