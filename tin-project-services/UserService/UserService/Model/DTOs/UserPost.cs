﻿namespace UserService.Model.DTOs;

public class UserPost
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public int RoleId { get; set; } // foreign key
}