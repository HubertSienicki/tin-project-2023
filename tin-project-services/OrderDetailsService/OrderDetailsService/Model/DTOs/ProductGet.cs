﻿namespace OrderDetailsService.Model.DTOs;

public class ProductGet
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
}