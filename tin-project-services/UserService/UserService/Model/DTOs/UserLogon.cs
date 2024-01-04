namespace UserService.Model.DTOs;

public class UserLogon
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    
    public int RoleId { get; set; } // foreign key
    public Role Role { get; set; }  // routing
}