﻿using MySql.Data.MySqlClient;
using UserService.Model;
using UserService.Model.DTOs;
using UserService.Repository.Interfaces;

namespace UserService.Repository.Persistance;

public class UserRepository : IUserRepository
{
    private readonly IConfiguration _configuration;


    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<User?>? GetUserById(int id)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        try
        {
            connection.Open();

            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT u1.Id, u1.username, u1.Email, u1.password, u1.password_salt, r1.roleId as RoleId, r1.Name as RoleName FROM Users u1 JOIN Roles r1 ON r1.roleId = u1.role_id WHERE u1.Id = {id}";

            Console.WriteLine(command.CommandText);

            await using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var user = new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    Salt = reader.GetString(reader.GetOrdinal("password_salt")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    Role = new Role
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("RoleId")),
                        Name = reader.GetString(reader.GetOrdinal("RoleName"))
                    }
                };

                return user;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    public async Task<UserLogon?> GetByUsername(string username)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();

            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT u1.Id, u1.username, u1.password, u1.password_salt, r1.roleId as RoleId, r1.Name as RoleName FROM Users u1 JOIN Roles r1 ON r1.roleId = u1.role_id WHERE u1.username = \'{username}\'";
            await using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var user = new UserLogon
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    Salt = reader.GetString(reader.GetOrdinal("password_salt")),
                    RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                    Role = new Role
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("RoleId")),
                        Name = reader.GetString(reader.GetOrdinal("RoleName"))
                    }
                };

                return user;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    public async Task<int> AddNewUser(UserPost userPost, string passwordSalt)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();

            await using var command = connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO Users (username, password, email, role_id, password_salt) VALUES(\'{userPost.Username}\', \'{userPost.Password}\', \'{userPost.Email}\', \'{userPost.RoleId}\', \'{passwordSalt}\')";
            var modified = await command.ExecuteNonQueryAsync();

            return modified;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return 0;
    }
}